using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySummaryText : MonoBehaviour
{
    private Button displayButton;
    private Canvas summaryCanvas;

    // Start is called before the first frame update
    void Start()
    {
        displayButton = GameObject.Find("DisplayButton").GetComponent<Button>();
        displayButton.onClick.AddListener(delegate { ShowText(true); });
    }

    // Update is called once per frame
    void ShowText(bool value)
    {
        
    }
}
