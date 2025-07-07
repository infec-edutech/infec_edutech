using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionQuestionController : MonoBehaviour
{
    [SerializeField] private TMP_Text _statement;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private Toggle[] _toggles;
    private Text[] _toogleTexts;

    private int _correctAnswer;
    private int _userSelection = -1;

    private Action<bool> onAnswer;

    private void Awake()
    {
        _confirmButton.onClick.AddListener(Confirm);
        _toogleTexts = _toggles.Select(o => o.GetComponentInChildren<Text>()).ToArray();
        for (int i = 0; i < _toggles.Length; i++)
            ToggleSelect(_toggles[i], i);
        gameObject.SetActive(false);
    }

    private void ToggleSelect(Toggle toggle, int selection)
    {
        toggle.onValueChanged.AddListener(value => { if (value) _userSelection = selection; });
    }

    private void Confirm()
    {
        if(_userSelection != -1)
            onAnswer.Invoke(_userSelection == _correctAnswer);
    }

    public void Setup(OptionsQuestionData data, Action<bool> onAnswerCallback)
    {
        gameObject.SetActive(true);
        _statement.text = data.statement;
        _toggleGroup.SetAllTogglesOff();
        _correctAnswer = UnityEngine.Random.Range(0, 3);
        _userSelection = -1;
        _toogleTexts[_correctAnswer].text = data.CorrectAnswer;
        var wronglist = new List<string>(data.WrongAnswers);
        for (int i = 0; i < _toogleTexts.Length; i++)
        {
            if (i == _correctAnswer) continue;
            var pickedindex = UnityEngine.Random.Range(0, wronglist.Count);
            _toogleTexts[i].text = wronglist[pickedindex];
            wronglist.RemoveAt(pickedindex);
            if (!wronglist.Any()) break;
        }
        onAnswer = onAnswerCallback;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
