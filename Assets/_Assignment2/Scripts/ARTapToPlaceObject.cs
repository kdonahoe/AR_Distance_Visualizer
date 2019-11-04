using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    ARRaycastManager raycastManager;

    public GameObject model;
    GameObject forklift;

    int frameCount = 0;

    Ray ray;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        forklift = Instantiate(model);
    }

    void Update()
    {        
        if (!getTouchPosition(out Vector2 pos))
        {
            return;
        }

        //enters if there is a touch on the screen
        if (raycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            //forklift faces towards finger point and moves towards it
            forklift.transform.LookAt(hitPose.position, Vector3.up);
            forklift.transform.position = Vector3.Lerp(forklift.transform.position, hitPose.position, 1f*Time.deltaTime);
        }
    }

    //taken from AR foundations sample
    bool getTouchPosition(out Vector2 pos)
    {
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            pos = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }

        pos = default;
        return false;
    }
}
