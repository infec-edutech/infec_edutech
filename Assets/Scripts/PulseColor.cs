using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseColor : MonoBehaviour
{
    [SerializeField] private Gradient _gradientLoop;
    [SerializeField] private float _loopDuration;
    [SerializeField] private Graphic _uiGraphic;

    private void Update()
    {
        if(_uiGraphic && _loopDuration != 0f)
            
            _uiGraphic.color = _gradientLoop.Evaluate(Mathf.Repeat(Time.timeSinceLevelLoad/_loopDuration,1f));
    }
}
