using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostStageSuccessController : MonoBehaviour
{
    [SerializeField] private TMP_Text _messageTxt;
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Image _start2;
    [SerializeField] private Image _start3;

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
    public void Setup(int seconds, Action<bool> callback)
    {
        gameObject.SetActive(true);

        var minutes = seconds / 60;
        seconds = seconds % 60;
        var message = $"Obrigado por jogar!!!\n" +
            $"Você jogou em {minutes}:{seconds} minutos\n" +
            $"Agora você conhece mais sobre as medidas de Precaução e é um colaborador do Controle de Infecção";

        _start2.enabled = seconds <= 300;
        _start3.enabled = seconds <= 90;
        _messageTxt.text = message;
        _callback = callback;
    }
}
