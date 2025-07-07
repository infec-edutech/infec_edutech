using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int _secondsPerQuestion = 120;

    [SerializeField] bool _debugMode;
    [SerializeField] GameObject _debugModeObj;
    [SerializeField] GameObject _bg;
    [SerializeField] Slider _timerSlider;
    [SerializeField] PreStageController _preStage;
    [SerializeField] AnswerResultPanel _answerResultPanel;
    [SerializeField] PostStageFailController _postStageFail;
    [SerializeField] PostStageSuccessController _postStageSuccess;
    [SerializeField] WordQuestionController _wordController;
    [SerializeField] LinkQuestionController _LinkController;
    [SerializeField] OptionQuestionController _OptionController;
    [SerializeField] StageScriptableObject[] _stages;

    bool _isHolding;
    bool _isSuccess;

    private void Awake()
    {
        _bg.SetActive(false);
        _timerSlider.gameObject.SetActive(false);
        _debugModeObj.SetActive(_debugMode);
    }

    public void PlayStage(int stage)
    {
        StartCoroutine(StartStage(_stages[stage]));
    }
    private IEnumerator StartStage(StageScriptableObject stage)
    {
        _bg.SetActive(true);

        _isHolding = true;
        _preStage.Setup(stage, Finish);
        yield return new WaitWhile(() => _isHolding);

        var questions = _debugMode ? stage.GetAllQueue() : stage.GetShuffledQueue(5);
        _isSuccess = false;
        //start animation?
        var startTime = Time.realtimeSinceStartup;
        QuestionData currentQuestion = null;
        while (questions.Any())
        {
            //Count down animation?
            currentQuestion = questions.Dequeue();
            yield return ShowQuestion(currentQuestion);
            if (_isSuccess || _debugMode)
            {
                //success animation
                
            }
            else
            {
                questions.Clear();
                //Fail animation
            }
            yield return _answerResultPanel.ShowResult(_isSuccess);
        }
        _isHolding = true;
        if (_isSuccess)
        {
            //Stage Success
            int elapsedTime = (int)(Time.realtimeSinceStartup - startTime);

            _postStageSuccess.Setup(elapsedTime, Finish);
        }
        else
        {
            //Stage Fail
            _postStageFail.Setup(currentQuestion, Finish);
        }
        yield return new WaitWhile(() => _isHolding);

        _bg.SetActive(false);

    }

    private IEnumerator ShowQuestion(QuestionData question)
    {
        _isHolding = true;

        var timer = StartCoroutine(Timer(_secondsPerQuestion));
        if (question is WordQuestionData wordQuestion)
        {
            _wordController.Setup(wordQuestion, Finish);
            yield return new WaitWhile(() => _isHolding);
            _wordController.Close();
        }
        else if (question is LinkQuestionData linkQuestion)
        {
            _LinkController.Setup(linkQuestion, Finish);
            yield return new WaitWhile(() => _isHolding);
            _LinkController.Close();
        }
        else if (question is OptionsQuestionData optionQuestion)
        {
            _OptionController.Setup(optionQuestion, Finish);
            yield return new WaitWhile(() => _isHolding);
            _OptionController.Close();
        }
        StopCoroutine(timer);
        _timerSlider.gameObject.SetActive(false);
    }

    void Finish(bool value)
    {
        _isSuccess = value;
        _isHolding = false;
    }

    private IEnumerator Timer(int seconds)
    {
        _timerSlider.gameObject.SetActive(true);
        _timerSlider.maxValue = seconds;
        _timerSlider.SetValueWithoutNotify(0f);
        var elapsed = 0;
        while (elapsed < seconds)
        {
            yield return new WaitForSeconds(1f);
            _timerSlider.SetValueWithoutNotify(++elapsed);
        }
        Finish(false);
    }
}