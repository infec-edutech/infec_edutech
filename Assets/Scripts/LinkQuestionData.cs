using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinkQuestionData : QuestionData
{
    [System.Serializable]
    public struct LinkPair
    {
        [TextArea(1, 10)]
        public string column1;
        [TextArea(1,10)]
        public string column2;
    }

    public LinkPair[] pairs;
}