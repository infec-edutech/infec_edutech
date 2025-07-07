using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreStageController : MonoBehaviour
{
    [SerializeField] private TMP_Text _stageTitleTxt;
    [SerializeField] private TMP_Text _stageMessageTxt;
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

    public void Setup(StageScriptableObject stage, Action<bool> callback)
    {
        gameObject.SetActive(true);
        _stageTitleTxt.text = stage.stageName;
        _stageMessageTxt.text = stage.stageDescription;
        _callback = callback;
    }
}
