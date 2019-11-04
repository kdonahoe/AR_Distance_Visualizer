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
    public List<GameObject> backupCubes = new List<GameObject>(); //for redo
    public List<GameObject> cubeShadows = new List<GameObject>();

    public List<GameObject> lines = new List<GameObject>();
    public List<GameObject> shadowLines = new List<GameObject>();

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject currentCube { get; private set; }
    public GameObject currentLine { get; private set; }
    public GameObject currentCubeShadow { get; private set; }

    int cubeCount = 0;
    int lineCount = 0;
    int shadowCount = 0;

    //prefabs
    public GameObject placedCube;
    public GameObject cubeShadow;

    public GameObject DistanceVisualizer;
    public GameObject DistanceVisualizerShadow;

    public GameObject mainLine;

    GameObject resetButton;
    GameObject placeButton;
    GameObject undoButton;
    GameObject redoButton;

    bool reset;
    bool place;
    bool undo;
    bool redo;

    bool touchedLast;

    public Scrollbar scroll;
    float scrollVal;

    public Vector3 cubeVelocity = Vector3.zero;

    Ray shadowRay;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        camera = GameObject.Find("AR Camera");

        resetButton = GameObject.Find("ResetButton");
        placeButton = GameObject.Find("PlaceButton");
        undoButton = GameObject.Find("UndoButton");
     //   redoButton = GameObject.Find("RedoButton");

        currentCube = Instantiate(placedCube, new Vector3(0, 0, 0), camera.transform.rotation);

        //creates random color for the cube
        var cubeRenderer = currentCube.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

        currentCubeShadow = Instantiate(cubeShadow);
        Instantiate(mainLine);
    }

    // Update is called once per frame
    void Update()
    {
        cubeCount = cubes.Count;
        lineCount = lines.Count;
        shadowCount = cubeShadows.Count;

        reset = resetButton.GetComponent<buttonController>().resetCube;
        place = placeButton.GetComponent<buttonController>().placeCube;
        undo = undoButton.GetComponent<buttonController>().undo;
        // redo = redoButton.GetComponent<buttonController>().redo;

        scrollVal = scroll.GetComponent<Scrollbar>().value;

        if (reset)
        {
            resetCubes();
        }

        if (place)
        {
            placeCube();
        }

        if (undo)
        {
            undoCube();
        }

        if (redo)
        {
            redoCube();
        }

        //moves cube attached to line
        currentCube.transform.position = Vector3.SmoothDamp(currentCube.transform.position, camera.transform.position + (1f + 3 * scrollVal) * camera.transform.forward, ref cubeVelocity, 0.8F);

        //finds where to add the shadow for the cube attached to the line
        shadowRay = new Ray(currentCube.transform.position, Vector3.down);
        if (raycastManager.Raycast(shadowRay, hits, TrackableType.PlaneWithinPolygon))
        {
            currentCubeShadow.active = true;
            var hitPose = hits[0].pose;
            currentCubeShadow.transform.position = hitPose.position;
            currentCubeShadow.transform.rotation = hitPose.rotation;
        }
        else
        {
            currentCubeShadow.active = false;
        }
    }

    private void placeCube()
    {
        placeButton.GetComponent<buttonController>().setPlaceFalse();

        //sets color of new cube to the cube attached to the line
        GameObject newPlacedCube = Instantiate(placedCube, currentCube.transform.position, currentCube.transform.rotation);
        var mainCubeRenderer = currentCube.GetComponent<Renderer>();
        var cubeRenderer = newPlacedCube.GetComponent<Renderer>();
        cubeRenderer.material.color =  mainCubeRenderer.material.GetColor("_Color");

        cubes.Add(newPlacedCube);
        backupCubes.Add(newPlacedCube);

        mainCubeRenderer.material.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

        //adds shadow to the new cube
        shadowRay = new Ray(currentCube.transform.position, Vector3.down);
        if (raycastManager.Raycast(shadowRay, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            GameObject newCubeShadow = Instantiate(cubeShadow, hitPose.position, currentCube.transform.rotation);
            cubeShadows.Add(newCubeShadow);
        }

        if (cubeCount >= 1)
        {
            GameObject line = Instantiate(DistanceVisualizer);
            lines.Add(line);

            GameObject shadowLine = Instantiate(DistanceVisualizerShadow);
            shadowLines.Add(shadowLine);
        }
    }

    void resetCubes()
    {
        resetButton.GetComponent<buttonController>().setResetFalse();

        for (int i = 0; i < cubeCount; i++)
        {
            Destroy(cubes[i]);

            if (i < lineCount)
            {
                Destroy(lines[i]);
                Destroy(lines[i].GetComponent<LineController>().distanceText);
            }
        }

        for (int i = 0; i < shadowCount; i++)
        {
            Destroy(cubeShadows[i]);
        }

        for (int i = 0; i < shadowLines.Count; i++)
        {
            Destroy(shadowLines[i]);
        }
        
        cubes.Clear();
        cubeShadows.Clear();

        backupCubes.Clear();

        lines.Clear();
        shadowLines.Clear();
    }

    void undoCube()
    {
        undoButton.GetComponent<buttonController>().setUndoFalse();

        if (cubeCount >= 0)
        {
            Destroy(cubes[cubeCount - 1]);
            Destroy(cubeShadows[shadowCount - 1]);

            Destroy(lines[lineCount - 1]);
            Destroy(shadowLines[lineCount - 1]);
            Destroy(lines[lineCount - 1].GetComponent<LineController>().distanceText);

            cubes.Remove(cubes[cubeCount - 1]);
            cubeShadows.Remove(cubeShadows[shadowCount - 1]);

            lines.Remove(lines[lineCount - 1]);
            shadowLines.Remove(lines[lineCount - 1]);
        }

    }

    void redoCube()
    {
        redoButton.GetComponent<buttonController>().setRedoFalse();

        if (backupCubes.Count > cubeCount)
        {
           GameObject lastCube = backupCubes[cubeCount];
           GameObject readdedCube = Instantiate(placedCube, lastCube.transform.position, lastCube.transform.rotation);

           cubes.Add(readdedCube);

        }
    }

}

