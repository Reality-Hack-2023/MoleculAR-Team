using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


public class OpenSensorPanel : MonoBehaviour
{
    [SerializeField]
    private Button greenButton;
    
    [SerializeField]
    private Canvas androidSensorCanvas;
    private int count = 0;


    // Start is called before the first frame update
    void Start()
    {
        greenButton = GameObject.Find("GreenButton").GetComponent<Button>();
        androidSensorCanvas = GameObject.Find("AndroidSensorCanvas").GetComponent<Canvas>();
        greenButton.onClick.AddListener(delegate { OpenCanvas(greenButton); });
    }

    // Update is called once per frame
    void OpenCanvas(Button buttonStatus)
    {
        count += 1;
    }

    private void Update()
    {
        if (count % 2 == 0)
        {
            androidSensorCanvas.enabled = false;

        }
        else
        {
            androidSensorCanvas.enabled = true;
        }
    }
}
