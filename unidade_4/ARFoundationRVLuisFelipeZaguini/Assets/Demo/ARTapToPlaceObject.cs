using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    private ARRaycastManager ARRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    // Start is called before the first frame update
    void Start()
    {
        ARRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void UpdatePlacementPose() {
        var screnCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        ARRaycastManager.Raycast(screnCenter, hits, TrackableType.AllTypes);

        placementPoseIsValid = hits.Count > 0;

        if(placementPoseIsValid) {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator() {
        placementIndicator.SetActive(placementPoseIsValid);
        placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if(placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        Vector3 EulerRotation = placementPose.rotation.eulerAngles;
        Instantiate(objectToPlace, placementPose.position, Quaternion.Euler(EulerRotation.x, 180, EulerRotation.z));
    }
}
