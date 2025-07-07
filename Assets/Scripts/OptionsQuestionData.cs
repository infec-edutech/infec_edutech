using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsQuestionData : QuestionData
{
    [TextArea(1,10)]
    public string CorrectAnswer;
    [TextArea(1,10)]
    public string[] WrongAnswers;
}