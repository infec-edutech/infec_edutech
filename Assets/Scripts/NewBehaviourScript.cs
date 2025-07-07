using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public string regex;
    public string resposta;

    [ContextMenu("test")]
    private void Test()
    {
        Debug.Log(Regex.IsMatch(resposta.ToLower(), regex));
    }
}
