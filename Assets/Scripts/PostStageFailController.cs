using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostStageFailController : MonoBehaviour
{
    [SerializeField] private TMP_Text _questionTxt;
    [SerializeField] private TMP_Text _correctAnswerTxt;
    [SerializeField] private Button _confirmBtn;

    private Action<bool> _callback;

    private void Awake()
    {
        _confirmBtn.onClick.AddListener(ExecuteCallback);
        gameObject.SetActive(false);
    }
    private void ExecuteCallback()
    {
        _callback?.Invoke(true);
        _callback = null;
        gameObject.SetActive(false);
    }
    public void Setup(QuestionData question, Action<bool> callback)
    {
        gameObject.SetActive(true);
        switch (question)
        {
            case null: _callback?.Invoke(false); return;
            case OptionsQuestionData optionQuestion:
                _correctAnswerTxt.text = optionQuestion.CorrectAnswer;
                break;
            case WordQuestionData wordQuestion:
                _correctAnswerTxt.text = wordQuestion.CorrectAnswer;
                break;
            case LinkQuestionData linkQuestion:
                string answer = "";
                foreach(var pair in linkQuestion.pairs)
                {
                    answer += $"{pair.column1} => {pair.column2}\n";
                }
                _correctAnswerTxt.text = answer.TrimEnd('\n');
                break;
        }
        _questionTxt.text = question.statement;
        _callback = callback;
    }
}
