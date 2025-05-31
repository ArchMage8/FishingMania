using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class LightCycleTest : MonoBehaviour
{
    [SerializeField] private AnimationCurve valueOverTimeCurve;
    [SerializeField] private float maxTime = 720f;
    
    [Space(20)]
    
    public float Display;


    [Space(40)]
    [Header("Built-In Calculator")]
    public float inputValue;
    public float manipulatedValue;

    

    private float elapsedTime = 0f;

    void Update()
    {
        if (elapsedTime < maxTime)
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = Mathf.Clamp01(elapsedTime / maxTime);
            float value = valueOverTimeCurve.Evaluate(normalizedTime);

            Display = value;

            //Debug.Log($"Time: {elapsedTime:F2}, Value: {value:F2}");

        }
    }

    private void OnValidate()
    {
        manipulatedValue = inputValue / 720f;
    }
}
