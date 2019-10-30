using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using UnityEngine.UI;
using UnityEngine;


[RequireComponent(typeof(ARRaycastManager))]
public class SceneController2 : MonoBehaviour
{
    ARRaycastManager raycastManager;

    GameObject camera;

    //lists to keep track of generates cubes and lines
    public List<GameObject> cubes = new List<GameObject>();
    public List<GameObject> lines = new List<GameObject>();
    public List<GameObject> shadows = new List<GameObject>();
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public GameObject currentCube { get; private set; }
    public GameObject currentLine { get; private set; }
    public GameObject currentShadow { get; private set; }

    int cubeCount = 0;
    int lineCount = 0;
    int shadowCount = 0;

    //prefabs
    public GameObject placedCube;    
    public GameObject DistanceVisualizer;
    public GameObject shadow;

    public GameObject mainLine;

    private GameObject resetButton;
    private GameObject undoButton;
    private GameObject placeButton;

    bool reset;
    bool undo;
    bool place;

    public Scrollbar scroll;
    float scrollVal;

    public Vector3 cubeVelocity = Vector3.zero;

    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        camera = GameObject.Find("AR Camera");

        resetButton = GameObject.Find("ResetButton");
        undoButton = GameObject.Find("UndoButton");
        placeButton = GameObject.Find("PlaceButton");


        currentCube = Instantiate(placedCube, new Vector3(0, 0, 0), camera.transform.rotation);
        currentShadow = Instantiate(shadow);
        Instantiate(mainLine);
    }

    // Update is called once per frame
    void Update()
    {
        cubeCount = cubes.Count;
        lineCount = lines.Count;
        shadowCount = shadows.Count;
        reset = resetButton.GetComponent<resetScene>().resetCube;
        undo = undoButton.GetComponent<UndoCube>().undo;
        place = placeButton.GetComponent<newCube>().placeCube;
        scrollVal = scroll.GetComponent<Scrollbar>().value;

        if (reset)
        {
            resetCubes();
        }

        if (undo)
        {
            undoCube();
        }

        if(place)
        {
            placeCube();
        }

        //moves cube attached to line
        currentCube.transform.position = Vector3.SmoothDamp(currentCube.transform.position, camera.transform.position + (1f + 3 * scrollVal) * camera.transform.forward, ref cubeVelocity, 0.8F);

        //finds where to add the shadow for the cube attached to the line
        ray = new Ray(currentCube.transform.position, Vector3.down);
        if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            currentShadow.transform.position = hitPose.position;
            currentShadow.transform.rotation = hitPose.rotation;
        }
    }

    private void placeCube()
    {
        placeButton.GetComponent<newCube>().setPlaceFalse();
        GameObject newPlacedCube = Instantiate(placedCube, currentCube.transform.position, currentCube.transform.rotation);
        cubes.Add(newPlacedCube);

        ray = new Ray(currentCube.transform.position, Vector3.down);
        if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            GameObject newPlacedShadow = Instantiate(shadow, hitPose.position, currentCube.transform.rotation);
            shadows.Add(newPlacedShadow);
        }

        if (cubeCount >= 1)
        {
            GameObject line = Instantiate(DistanceVisualizer);
            lines.Add(line);
        }
    }

    void resetCubes()
    {
        resetButton.GetComponent<resetScene>().setResetFalse();

        for (int i = 0; i < cubeCount; i++)
        {
            Destroy(cubes[i]);
            
            if (i < lineCount)
            {
                Destroy(lines[i]);
                Destroy(lines[i].GetComponent<LineController>().distanceText);
            }
        }

        for(int i = 0; i<shadowCount;i++)
        {
            Destroy(shadows[i]);
        }
        shadows.Clear();
        cubes.Clear();
        lines.Clear();
    }

    void undoCube()
    {
        undoButton.GetComponent<UndoCube>().setUndoFalse();

        if (cubeCount >= 0)
        {
            Destroy(cubes[cubeCount - 1]);
            Destroy(lines[lineCount - 1]);
            Destroy(lines[lineCount - 1].GetComponent<LineController>().distanceText);
            Destroy(shadows[shadowCount - 1]);

            cubes.Remove(cubes[cubeCount - 1]);
            lines.Remove(lines[lineCount - 1]);
            shadows.Remove(shadows[shadowCount - 1]);
        }

    }


}

