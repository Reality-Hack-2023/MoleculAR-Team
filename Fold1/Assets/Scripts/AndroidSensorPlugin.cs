using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class AndroidSensorPlugin : MonoBehaviour
{
    [SerializeField]
    private TMP_Text xaccel, yaccel, zaccel; 

    

    private AndroidJavaClass unityClass;
    
    private AndroidJavaObject unityActivity;
    private AndroidJavaObject _pluginInstance;


    void Start()
    {
        InitializePlugin("explore.project.androidplugin1");
        _pluginInstance.Call("StartSensorListening");
        xaccel = GameObject.Find("XAccel").GetComponent<TMP_Text>();
        yaccel = GameObject.Find("YAccel").GetComponent<TMP_Text>();
        zaccel = GameObject.Find("ZAccel").GetComponent<TMP_Text>();

    }

    void ApplicationOnPause()
    {
        if (_pluginInstance != null)
        {
            _pluginInstance.Call("onPause");
            _pluginInstance = null;
        }
    }

    void InitializePlugin(string pluginName)
    {

        unityClass = new AndroidJavaClass("explore.project.androidplugin1");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("UnitySensorPlugin");
        _pluginInstance = new AndroidJavaObject(pluginName);
        if (_pluginInstance == null)
        {
            Debug.LogError("Plugin Error");
        }
        _pluginInstance.CallStatic("receiveUnityActivity", unityActivity);
    }

    // Update is called once per frame
    void Update()
    {
        float[] xyz = _pluginInstance.Call<float[]>("getSensorValues", "accelerometer");
        

        xaccel.text = "X value:" + xyz[0];
        yaccel.text = "Y value:" + xyz[1];
        zaccel.text = "Z value:" + xyz[2];
    }

    public void Add()
    {
        if (_pluginInstance != null)
        {
            var result = _pluginInstance.Call<int>("Add", 10, 10);
            _pluginInstance.Call("Toast", result.ToString());
        }
    }



    






}
