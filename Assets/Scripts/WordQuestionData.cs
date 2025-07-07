using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordQuestionData : QuestionData
{
    [TextArea(1,10)]
    public string CorrectAnswer;
    public string Regex;
}
