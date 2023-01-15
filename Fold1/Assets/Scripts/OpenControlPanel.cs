using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenControlPanel : MonoBehaviour
{
    private Button openPanelButton;
    public Canvas controlPanelCanvas;
    private int count = 0;


    // Start is called before the first frame update
    void Start()
    {
        openPanelButton = GameObject.Find("OpenButton").GetComponent<Button>();
        controlPanelCanvas = GameObject.Find("ControlPanelCanvas").GetComponent<Canvas>();
        openPanelButton.onClick.AddListener(delegate { OpenCanvas(openPanelButton); });
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
            controlPanelCanvas.enabled = false;
            
        }
        else
        {
            controlPanelCanvas.enabled = true;
        }
    }
}
