using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]

public class PlacementController : MonoBehaviour
{   
    [SerializeField]
    private Toggle planeToggle;
    [SerializeField]
    private TMP_Dropdown dropdownBox;
    [SerializeField]
    private Button addButton;
    [SerializeField]
    private TMP_Text placementText;

    private GameObject lastSelectedMolecule;

    private GameObject placedMolecule;
    private GameObject prefabModel;
   
    private bool isSwitching = false;
    private double min_distance = 300;

    [SerializeField]
    private List<GameObject> MoleculeList;

    private Vector2 touchPosition = default;
    private string currentName1;

    private ARPlaneManager arPlaneManager;
    private ARRaycastManager arRaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        arPlaneManager = GetComponent<ARPlaneManager>();


        planeToggle = GameObject.Find("PlaneToggle").GetComponent<Toggle>();
        planeToggle.onValueChanged.AddListener(delegate { TogglePlaneDetection(planeToggle); });
        

        dropdownBox = GameObject.Find("NameDropdown").GetComponent<TMP_Dropdown>();
        dropdownBox.onValueChanged.AddListener(delegate { SelectMolecule(dropdownBox); });
        

        addButton = GameObject.Find("AddButton").GetComponent<Button>();
        addButton.onClick.AddListener(delegate { CreateMolecule(addButton); });

        placementText = GameObject.Find("PlacementText").GetComponent<TMP_Text>();


        if (!planeToggle.isOn)
        {
            SetAllPlaneActive(false);
        }
    }

    public void CreateMolecule(Button addbutton)
    {
        isSwitching = true;
        placementText.text += "switching";
    }


    public void TogglePlaneDetection(Toggle toggleStatus)
    {
        if (toggleStatus.isOn)
        {
            SetAllPlaneActive(true);
            arPlaneManager.enabled = true;
        }
        else
        {
            SetAllPlaneActive(false);
            arPlaneManager.enabled = false;
        }
        
    }

    public void SelectMolecule(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        currentName1 = dropdown.options[index].text;
        prefabModel = Resources.Load<GameObject>($"Prefabs/{currentName1}");

        if (prefabModel == null)
        {
            placementText.text += "prefabModel is null";
        }
        else 
        {
            lastSelectedMolecule = null;
        }
    }

    private void SetAllPlaneActive(bool value)
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
        
    }

    
    void Update()
    {
        if (Input.touchCount > 0)
            
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began && !isOverlappingTouch())
            {
                if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    if (lastSelectedMolecule == null)
                    {
                        lastSelectedMolecule = Instantiate(prefabModel, hitPose.position, hitPose.rotation);

                        Canvas newCanvas = Resources.Load<Canvas>("Canvas/newCanvas");
                        newCanvas.transform.SetParent(lastSelectedMolecule.transform);
                        newCanvas.transform.position = lastSelectedMolecule.transform.position;
                        newCanvas.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
                        newCanvas.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);

                        MoleculeList.Add(lastSelectedMolecule);
                        placementText.text += " Add ";
                    }
                    else
                    {
                        if (isSwitching)
                        {
                            PointerEventData eventCurrentPosition = new PointerEventData(EventSystem.current);
                            eventCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                            if (arRaycastManager.Raycast(eventCurrentPosition.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                            {
                                Pose switchHit = hits[0].pose;
                                lastSelectedMolecule = Search(switchHit.position, MoleculeList);
                                placementText.text += " switch ";
                                isSwitching = false;
                            }
                        }
                        else 
                        {
                            lastSelectedMolecule.transform.position = hitPose.position;                                                         
                        }
                    }
                }
            }
        }
    }

    private double EucledianDistance(float x1, float x2, float y1, float y2 )
    {
        double x = System.Math.Pow((x1 - x2), 2);
        double y = System.Math.Pow((y1 - y2), 2);
        

        double distance = System.Math.Pow((x + y), 0.5f);
        return distance/10;
    }

    private GameObject Search(Vector3 pos, List<GameObject> allMolecules)
    {
        min_distance = 1000;
        GameObject selectedMol = lastSelectedMolecule;
        foreach (GameObject mol in allMolecules)
        {
            double temp = EucledianDistance(pos.x, mol.transform.position.x, pos.y, mol.transform.position.y);
            placementText.text += min_distance.ToString() + " ";
            if (temp < min_distance)
            {
                min_distance = temp;
                selectedMol = mol;
                
            }
        }

        return selectedMol;
    }


    private bool isOverlappingTouch()
    {
        PointerEventData eventCurrentPosition = new PointerEventData(EventSystem.current);
        eventCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        List<RaycastResult> rayCastList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventCurrentPosition, rayCastList);
        return rayCastList.Count > 0;
    }

    
}
