using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/StageScriptableObject")]
public class StageScriptableObject : ScriptableObject
{
    public string stageName;
    [TextArea(3,10)]public string stageDescription;

    public OptionsQuestionData[] optionsQuestions;
    public LinkQuestionData[] linkQuestions;
    public WordQuestionData[] wordQuestions;

    public Queue<QuestionData> GetShuffledQueue(int quantity)
    {
        List<QuestionData> allQuestions = new List<QuestionData>();
        allQuestions.AddRange(optionsQuestions);
        allQuestions.AddRange(linkQuestions);
        allQuestions.AddRange(wordQuestions);
        Shuffle(allQuestions);
        return new Queue<QuestionData>(allQuestions.GetRange(0, quantity));
    }

    public Queue<QuestionData> GetAllQueue()
    {
        List<QuestionData> allQuestions = new List<QuestionData>();
        allQuestions.AddRange(optionsQuestions);
        allQuestions.AddRange(linkQuestions);
        allQuestions.AddRange(wordQuestions);
        return new Queue<QuestionData>(allQuestions);
    }

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}