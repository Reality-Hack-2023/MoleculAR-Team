using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;


public class ClusteringModel : MonoBehaviour
{
    [SerializeField]
    private Button umapButton;

    [SerializeField]
    private GameObject root;
    [SerializeField]
    private GameObject chemicalSpace;

    [SerializeField]
    private TMP_Text analyticalText;

    private Slider scaleSlider;
    private Slider rotationSlider;

    public float scaleMinValue;
    public float scaleMaxValue;

    public float rotationMinValue;
    public float rotationMaxValue;

    private List<GameObject> samples;
    private GameObject sphere, obj;
    private int clickCount = 0;
    private int i = 0;

    private List<float> xValues;
    private List<float> yValues;
    private List<float> zValues;
    private List<float> labelValues;
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        chemicalSpace = GameObject.Find("chemicalSpace");
        root = GameObject.Find("root");
        samples = new List<GameObject>();

        sphere = Resources.Load<GameObject>("Prefabs/Sphere");

        umapButton = GameObject.Find("UMAPButton").GetComponent<Button>();
        umapButton.onClick.AddListener(delegate { ReadCSVFile(umapButton); });

        analyticalText = GameObject.Find("AnalyticalText").GetComponent<TMP_Text>();

        root = GameObject.Find("root");
        scaleSlider = GameObject.Find("ClusterlScaler").GetComponent<Slider>();
        scaleSlider.minValue = scaleMinValue;
        scaleSlider.maxValue = scaleMaxValue;
        scaleSlider.onValueChanged.AddListener(ClusterScaleSlider);

        rotationSlider = GameObject.Find("ClusterRotation").GetComponent<Slider>();
        rotationSlider.minValue = rotationMinValue;
        rotationSlider.maxValue = rotationMaxValue;
        rotationSlider.onValueChanged.AddListener(ClusterRotationSliderUpdate);



    }

    void ClusterScaleSlider(float value)
    {
        chemicalSpace.transform.localScale = new Vector3(value, value, value);
    }

    void ClusterRotationSliderUpdate(float value)
    {
        chemicalSpace.transform.localEulerAngles = new Vector3(transform.rotation.x, value, transform.rotation.z);
    }

    public void ReadCSVFile(Button yellowB) 
    {
        clickCount += 1;

        TextAsset txt = (TextAsset)Resources.Load("File/UMAP_solubility", typeof(TextAsset));
        string filecontent = txt.text;
        string[] lines = filecontent.Split("\n");
        Debug.Log(lines[0]);
        float xRoot = 0;
        float yRoot = 0;
        float zRoot = 0;
        Quaternion rootQuat = new Quaternion(0, 0, 0, 0);

        while (i<lines.Length) 
        {
            string[] values = lines[i].Split(",");

            float x = float.Parse(values[0]);
            float y = float.Parse(values[1]);
            float z = float.Parse(values[2]);
            float label = float.Parse(values[3]);
            if (i == 0)
            {
                xRoot = x;
                yRoot = y;
                zRoot = z;

                Vector3 rootPos = new Vector3(x, y, z);

                root.transform.position = rootPos;
                if (label == 0)
                {
                    root.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    root.GetComponent<Renderer>().material.color = Color.blue;
                }

            }
            else 
            {
                obj = Instantiate(sphere, new Vector3(x - xRoot, y - yRoot, z -zRoot), rootQuat);
                obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                if (label == 0)
                {
                    obj.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    obj.GetComponent<Renderer>().material.color = Color.blue;
                }
                obj.transform.name = $"Sphere {i}";
                obj.transform.SetParent(root.transform);
            }
            

            i += 1;
        }
        root.transform.position = new Vector3(xRoot-6,yRoot-6,zRoot-6);
        


    }

    // Update is called once per frame
    void Update()
    {
        /*/if (Input.touchCount > 0)
        {
            Vector2 touchPostion = Input.GetTouch(0).position;

            if (aRRaycastManager.Raycast(touchPostion, hits, TrackableType.FeaturePoint))
            {
                Pose hitPose = hits[0].pose;
                chemicalSpace.transform.position = hitPose.position;
            }

        
        }/*/
    }
}
