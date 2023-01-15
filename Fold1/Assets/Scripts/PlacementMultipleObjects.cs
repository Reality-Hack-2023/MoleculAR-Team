using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]

public class PlacementMultipleObjects : MonoBehaviour
{
    [SerializeField]

    private GameObject PlacablePrefab;
    private GameObject placedMolecule;
    private GameObject placedMoleculeInput;



    private ARRaycastManager arRaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if (placedMolecule == null)
            {
                placedMolecule = Instantiate(placedMoleculeInput, hitPose.position, hitPose.rotation);
            }
            else
            {
                placedMolecule.transform.position = hitPose.position;
                placedMolecule.transform.rotation = hitPose.rotation;
            }

        }
    }

    public void SetPrefabType(GameObject prefabType)
    {
        PlacablePrefab = prefabType;
    }


}
