using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinkQuestionController : MonoBehaviour 
{
    [SerializeField] private TMP_Text _statement;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private ToggleGroup[] _toggleGroup;
    [SerializeField] private Toggle[] _togglesColumn1;
    [SerializeField] private Toggle[] _togglesColumn2;
    [SerializeField] private LineRenderer[] _lineRenderers;
    
    private Text[] _toogleTextsColum1;
    private Text[] _toogleTextsColum2;

    private int _userSelectionColumn1 = -1;
    private int _userSelectionColumn2 = -1;

    private Action<bool> onAnswer;

    private Dictionary<LineRenderer,(int, int)> _linePairs;

    private HashSet<string> _matchingAnswer = new HashSet<string>();

    private int pairsCount;

    private void Awake()
    {
        _confirmButton.onClick.AddListener(Confirm);
        _toogleTextsColum1 = _togglesColumn1.Select(o => o.GetComponentInChildren<Text>()).ToArray();
        _toogleTextsColum2 = _togglesColumn2.Select(o => o.GetComponentInChildren<Text>()).ToArray();
        _linePairs = _lineRenderers.ToDictionary(o=>o, o=>(-1,-1));
        for (int i = 0; i < _togglesColumn1.Length; i++)
            SetUserColumn1(_togglesColumn1[i],i);
        for (int i = 0; i < _togglesColumn2.Length; i++)
            SetUserColumn2(_togglesColumn2[i],i);
        foreach (var toggleGroup in _toggleGroup)
            toggleGroup.SetAllTogglesOff();
        gameObject.SetActive(false);
    }

    private void SetUserColumn1(Toggle toggle, int selection)
    {
        toggle.onValueChanged.AddListener(value => { if (value) _userSelectionColumn1 = selection; TryPair(); });
    }
    private void SetUserColumn2(Toggle toggle, int selection)
    {
        toggle.onValueChanged.AddListener(value => { if (value) _userSelectionColumn2 = selection; TryPair(); });
    }

    private void TryPair()
    {
        if (_userSelectionColumn1 == -1 || _userSelectionColumn2 == -1) return;

        if(_linePairs.Any(o => o.Value.Item1 == _userSelectionColumn1 || o.Value.Item2 == _userSelectionColumn2))
        {
            var usedLines = _linePairs.Where(o => o.Value.Item1 == _userSelectionColumn1 || o.Value.Item2 == _userSelectionColumn2).Select(o=>o.Key).ToArray();
            foreach (var usedLine in usedLines)
            {
                usedLine.SetPosition(0, Vector3.zero);
                usedLine.SetPosition(1, Vector3.zero);
                _linePairs[usedLine] = (-1, -1);
            }
        }

        var line = _linePairs.First(o => o.Value.Item1 == -1 && o.Value.Item2 == -1).Key;

        _linePairs[line] = (_userSelectionColumn1, _userSelectionColumn2);

        line.SetPosition(0, _togglesColumn1[_userSelectionColumn1].transform.GetChild(0).position);
        line.SetPosition(1, _togglesColumn2[_userSelectionColumn2].transform.GetChild(0).position);

        _userSelectionColumn1 = _userSelectionColumn2 = -1;
        foreach(ToggleGroup group in _toggleGroup)
        {
            group.SetAllTogglesOff();
        }
    }

    private void Confirm()
    {
        if (_linePairs.Count(o => o.Value.Item1 != -1 && o.Value.Item2 != -1) < pairsCount)
            return;

        foreach (var match in _matchingAnswer)
            Debug.Log(match);

        foreach (var answer in _linePairs.Values.Where(o => o.Item1 != -1 && o.Item2 != -1))
        {
            Debug.Log($"{answer.Item1}    {answer.Item2}");
            if (_matchingAnswer.Add(answer.Item1.ToString()+ answer.Item2.ToString()))
            {
                onAnswer.Invoke(false);
                return;
            }
        }
        //foreach (var toggleGroup in _toggleGroup)
        //    toggleGroup.SetAllTogglesOff();
        onAnswer.Invoke(true);
    
    }
    public void Setup(LinkQuestionData data, Action<bool> onAnswerCallback)
    {
        gameObject.SetActive(true);
        _statement.text = data.statement;
        foreach (var toggleGroup in _toggleGroup)
            toggleGroup.SetAllTogglesOff();

        var pairslist = new List<LinkQuestionData.LinkPair>(data.pairs);
        pairsCount = data.pairs.Length;
        var matchinglist1 = new List<int>();
        var matchinglist2 = new List<int>();
        foreach (var toggle in _togglesColumn1.Concat(_togglesColumn2))
        {
            toggle.gameObject.SetActive(false);
        }
        for(int i = 0; i < data.pairs.Length; i++)
        {
            matchinglist1.Add(i);
            matchinglist2.Add(i);
            _togglesColumn1[i].gameObject.SetActive(true);
            _togglesColumn2[i].gameObject.SetActive(true);
        }
        _matchingAnswer.Clear();
        for (int i = 0; i < data.pairs.Length; i++)
        {
            var pickedindex = matchinglist1[UnityEngine.Random.Range(0, matchinglist1.Count)];
            _toogleTextsColum1[i].text = pairslist[pickedindex].column1;
            var pickedmatchindex = matchinglist2[UnityEngine.Random.Range(0, matchinglist2.Count)];
            _toogleTextsColum2[pickedmatchindex].text = pairslist[pickedindex].column2;

            _matchingAnswer.Add(i.ToString() + pickedmatchindex.ToString());
            matchinglist1.Remove(pickedindex);
            matchinglist2.Remove(pickedmatchindex);
        }
        foreach (var match in _matchingAnswer)
            Debug.Log(match);
        foreach (var line in _lineRenderers)
        {
            _linePairs[line] = (-1, -1);
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
        }
        onAnswer = onAnswerCallback;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}