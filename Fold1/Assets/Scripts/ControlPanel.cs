using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControlPanel : MonoBehaviour
{
    private Slider scaleSlider;
    private Slider rotationSlider;

    public float scaleMinValue;
    public float scaleMaxValue;

    public float rotationMinValue;
    public float rotationMaxValue;


    void Start()
    {
        scaleSlider = GameObject.Find("ScaleSlider").GetComponent<Slider>();
        scaleSlider.minValue = scaleMinValue;
        scaleSlider.maxValue = scaleMaxValue;
        scaleSlider.onValueChanged.AddListener(ScaleSliderUpdate);

        rotationSlider = GameObject.Find("RotationSlider").GetComponent<Slider>();
        rotationSlider.minValue = rotationMinValue;
        rotationSlider.maxValue = rotationMaxValue;
        rotationSlider.onValueChanged.AddListener(RotationSliderUpdate);
    }
    

    void ScaleSliderUpdate(float value) 
    {
        transform.localScale = new Vector3(value, value, value);
    }

    void RotationSliderUpdate(float value)
    {
        transform.localEulerAngles = new Vector3(transform.rotation.x, value, transform.rotation.z);
    }

}
