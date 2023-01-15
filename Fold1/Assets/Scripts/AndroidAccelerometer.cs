using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

public class AndroidAccelerometer : MonoBehaviour
{
	[SerializeField]
	private TMP_Text xAccel, yAccel, zAccel;

	private AndroidJavaObject plugin;

	void Start()
	{
		xAccel = GameObject.Find("XAccel").GetComponent<TMP_Text>();
		yAccel = GameObject.Find("YAccel").GetComponent<TMP_Text>();
		zAccel = GameObject.Find("ZAccel").GetComponent<TMP_Text>();
		

		plugin = new AndroidJavaClass("explore.project.androidplugin1.UnitySensorPlugin").CallStatic<AndroidJavaObject>("getInstance");
		plugin.Call("setSamplingPeriod", 100 * 1000); // refresh sensor 100 mSec each
		plugin.Call("startSensorListening", "accelerometer");

	}

	void OnApplicationQuit()
	{
		if (plugin != null)
		{
			plugin.Call("terminate");
			plugin = null;
		}
	}

	void Update()
	{

		if (plugin != null)
		{
			float[] sensorValue = plugin.Call<float[]>("getSensorValues", "accelerometer");
			if (sensorValue != null)
			{
				xAccel.text = "X value: " + sensorValue[0].ToString();
				yAccel.text = "Y value: " + sensorValue[1].ToString();
				zAccel.text = "Z value: " + sensorValue[2].ToString();

			}
		}
	}
}
