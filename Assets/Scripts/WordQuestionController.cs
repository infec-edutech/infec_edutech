using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordQuestionController : MonoBehaviour
{
    [SerializeField] private TMP_Text _statement;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private TMP_InputField _userInput;

    private string _correctAnswer;
    private string _regexPattern;

    private Action<bool> onAnswer;

    private void Awake()
    {
        _confirmButton.onClick.AddListener(Confirm);
        gameObject.SetActive(false);
    }

    private void Confirm()
    {
        if (!string.IsNullOrWhiteSpace(_userInput.text))
            onAnswer.Invoke(Regex.IsMatch(_userInput.text.ToLower(), _regexPattern));
    }

    public void Setup(WordQuestionData data, Action<bool> onAnswerCallback)
    {
        gameObject.SetActive(true);
        _statement.text = data.statement;
        _correctAnswer = data.CorrectAnswer;
        _regexPattern = data.Regex;
        _userInput.text = string.Empty;
        onAnswer = onAnswerCallback;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
