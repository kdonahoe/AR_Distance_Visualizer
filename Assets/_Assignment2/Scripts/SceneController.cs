using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;

[RequireComponent(typeof(ARRaycastManager))]
public class SceneController : MonoBehaviour
{
    ARRaycastManager raycastManager;

    //lists to keep track of generates cubes and lines
    public List<GameObject> cubes = new List<GameObject>();
    public List<GameObject> lines = new List<GameObject>();

    //list counts
    int cubeCount = 0;
    int lineCount = 0;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject currentCube { get; private set; }
    public GameObject currentLine { get; private set; }

    //prefabs
    public GameObject placedCube;
    public GameObject distanceVisualizer;

    //buttons
    private GameObject resetButton;
    private GameObject undoButton;

    //keeps track on whether button was pressed
    bool reset;
    bool undo;
    
    //used to manage and regulate touch input
    int frameCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        resetButton = GameObject.Find("ResetButton");
        undoButton = GameObject.Find("UndoButton");
    }

    // Update is called once per frame
    void Update()
    {
        cubeCount = cubes.Count;
        lineCount = lines.Count;

        //checks
        reset = resetButton.GetComponent<resetScene>().resetCube;
        undo = undoButton.GetComponent<UndoCube>().undo;

        //if reset, destroys all cubes and lines
        if (reset)
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
            cubes.Clear();
            lines.Clear();
        }

        //if undo, destroys most recent cube and associated line
        if (undo)
        {
            undoButton.GetComponent<UndoCube>().setUndoFalse();

            if (cubeCount > 0)
            {
                Destroy(cubes[cubeCount - 1]);
                Destroy(lines[lineCount - 1]);
                Destroy(lines[lineCount - 1].GetComponent<LineController>().distanceText);

                cubes.Remove(cubes[cubeCount - 1]);
                lines.Remove(lines[lineCount - 1]);
            }
        }

        //checks touch input; adds new cube if touched
        //runs every 6th time Update is called, to not "spam" the screen with cubes
        if (frameCount % 6 == 0)
        {
            //This getTouchPosition and raycast code was modified from the ARFoundations code
            if (!getTouchPosition(out Vector2 pos))
            {
                return;
            }

            if (raycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                currentCube = Instantiate(placedCube, hitPose.position, hitPose.rotation);
                cubes.Add(currentCube);
                cubeCount = cubes.Count;
                lineCount = lines.Count;
                if (cubeCount >= 2)
                {
                    currentLine = Instantiate(distanceVisualizer);
                    lines.Add(currentLine);
                }
            }
        }
        
        frameCount ++;
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

