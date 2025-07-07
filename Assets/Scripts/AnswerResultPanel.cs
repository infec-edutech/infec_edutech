using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnswerResultPanel : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] GameObject _success;
    [SerializeField] GameObject _fail;

    public IEnumerator ShowResult(bool success)
    {
        _success.SetActive(success);
        _fail.SetActive(!success);
        _canvasGroup.DOFade(1, 0.2f);
        yield return new WaitForSeconds(1f);
        _canvasGroup.DOFade(0, 0.2f);
    }

}
