using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PluginAndroid : MonoBehaviour
{
    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject _pluginInstance;

    [SerializeField]
    private ARRaycastManager aRRaycastManager;

    [SerializeField]
    private ARPlaneManager aRPlaneManager;

    [SerializeField]
    private TMP_Text compIdText;
    [SerializeField]
    private TMP_Text molecularFormularText;
    [SerializeField]
    private TMP_Text molecularWeightText;
    [SerializeField]
    private TMP_Text smilesText;
    [SerializeField]
    private TMP_Text inChlKeyText;
    [SerializeField]
    private TMP_Text symbolText;
    [SerializeField]
    private Button displayButton;

    [SerializeField]
    private Button toastButton;

    [SerializeField]
    private string s;

    [SerializeField]
    private string res;

    [SerializeField]
    private TMP_Dropdown dropdown;

    private string[] info;

    private string currentMoleculeId;

    public List<GameObject> samples;

    [SerializeField]
    private TMP_Text debugLog;



    void Start()
    {
        InitializePlugin("hackathon.project.unityplugin1.PluginInstance");

        dropdown = GameObject.Find("NameDropdown").GetComponent<TMP_Dropdown>();
        

        compIdText = GameObject.Find("CompoundCID").GetComponent<TMP_Text>();
        molecularFormularText = GameObject.Find("Molecular Fomular").GetComponent<TMP_Text>();
        molecularWeightText = GameObject.Find("MolecularWeight").GetComponent<TMP_Text>();
        smilesText = GameObject.Find("Smiles").GetComponent<TMP_Text>();
        inChlKeyText = GameObject.Find("InChIKey").GetComponent<TMP_Text>();
        symbolText = GameObject.Find("Symbol").GetComponent<TMP_Text>();


        displayButton = GameObject.Find("DisplayButton").GetComponent<Button>();
        displayButton.onClick.AddListener(delegate { UpdateCanvas(displayButton);});

        toastButton = GameObject.Find("ToastButton").GetComponent<Button>();
        toastButton.onClick.AddListener(delegate { Toast(); });

        info = null;
        
                
        
        debugLog = GameObject.Find("DebugText").GetComponent<TMP_Text>();

        if (_pluginInstance != null)
        {
            debugLog.text += "Plugin ok ";
        }
        else
        {
            debugLog.text += "Bad plugin ";
        }


    }

    public void StartSensor() 
    {
        if (_pluginInstance != null) 
        {
            _pluginInstance.Call("StartSensor");
        }
    }

    void Update()
    {
        
    }


    void InitializePlugin(string pluginName)
    {
        
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        _pluginInstance = new AndroidJavaObject(pluginName);
        if (_pluginInstance == null)
        {
            Debug.LogError("Plugin Error");
        }
        _pluginInstance.CallStatic("receiveUnityActivity", unityActivity);
    }

    public void AddInt() 
    {
        if (_pluginInstance != null) 
        {
            var result = _pluginInstance.Call<int>("Add", 10, 10);
            debugLog.text += result.ToString();
        }
    }


    public void Toast()
    {
        string id = "";
        if (currentMoleculeId == "D1")
        {
            id = "CFF";
        } 
        else if (currentMoleculeId == "Y")
        {
            id = "PN7";
        }
        else if (currentMoleculeId == "W")
        {
            id = "CN7";
        }
        /*/debugLog.text += id;/*/

        s = _pluginInstance.Call<string>("ShowProperties", id);
        
        res = _pluginInstance.Call<string>("extractInfo", s);
        _pluginInstance.Call("Toast", res);
        info = res.Split("***");

        



    }

    public void CreateTarget(TMP_Dropdown dropdownBar)
    {
        int index = dropdownBar.value;
        currentMoleculeId = dropdownBar.options[index].text;

    }

    public async void UpdateCanvas(Button dButton)
    {
        int index = dropdown.value;
        currentMoleculeId = dropdown.options[index].text;
        debugLog.text += currentMoleculeId;
        _pluginInstance.Call("Toast", currentMoleculeId);
        info = null;
        while (info == null)
        {
            Toast();
            string l = info.Length.ToString();
            
            await Task.Yield();
            _pluginInstance.Call("Toast", "finished");
        }
    }


}
