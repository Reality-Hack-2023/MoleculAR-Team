using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class PlacementMNG : MonoBehaviour
{
    public GameObject ObjectAnchor;

    [SerializeField]
    private ARSessionOrigin aRSessionOrigin;

    [SerializeField]
    private ARRaycastManager aRRaycastManager;

    [SerializeField]
    private TrackableType trackableType = TrackableType.Planes;

    private Pose placementPose;
    private bool isValidPose = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
    }

    private void UpdatePlacementIndicator()
    {
        if (isValidPose)
        {
            ObjectAnchor.SetActive(true);
            ObjectAnchor.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
    }


    private void UpdatePlacementPose()
    {
        var screenCenter = aRSessionOrigin.camera.ViewportPointToRay(new Vector3(0.5f,0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, trackableType);
        isValidPose = hits.Count>0;
        if (hits.Count > 0)
        {
            placementPose = hits[0].pose;

            var cameraForward = aRSessionOrigin.camera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

        }




    }
}
