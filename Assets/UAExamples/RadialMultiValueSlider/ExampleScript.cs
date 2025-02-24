using UnityEngine;
using System.Collections;

public class ExampleScript : MonoBehaviour
{
    public RadialMultiValueSlider slider;
    void OnGUI ()
    {
        foreach (var v in slider.values)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(v.name, GUILayout.Width(150));
            GUILayout.Label(""+(v.actualValue*100).ToString("00.0000")+"%");
            GUILayout.EndHorizontal();
        }
    }
}
