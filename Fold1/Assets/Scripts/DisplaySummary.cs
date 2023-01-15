using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


public class DisplaySummary : MonoBehaviour
{
    private Toggle summaryToggle;
    public Canvas summaryCanvas;

    private void Start()
    {
        summaryCanvas.enabled = false;
        summaryToggle = GameObject.Find("SummaryToggle").GetComponent<Toggle>();
        summaryToggle.onValueChanged.AddListener(delegate { ShowSummary(summaryToggle); } );
    }

    
    public void ShowSummary(Toggle toggleStatus) 
    {
        if (toggleStatus.isOn)
        {
            summaryCanvas.enabled = true;
        }
        else
        {
            summaryCanvas.enabled = false;
        }

    }
}
