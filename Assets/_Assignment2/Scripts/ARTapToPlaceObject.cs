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
    GameObject bee;
    int frameCount = 0;
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        bee = Instantiate(model);
    }

    // Update is called once per frame
    void Update()
    {        
        if (!getTouchPosition(out Vector2 pos))
        {
            return;
        }

        if (raycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            bee.transform.LookAt(hitPose.position, Vector3.up);
            bee.transform.position = Vector3.Lerp(bee.transform.position, hitPose.position, 1f*Time.deltaTime);
        }
    }

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
