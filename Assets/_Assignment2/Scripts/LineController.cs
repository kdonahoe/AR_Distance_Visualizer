using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LineController : MonoBehaviour
{
    LineRenderer lineRenderer;

    private GameObject session;

    public GameObject distanceTextPrefab;
    public GameObject distanceText;
    
    List<GameObject> cubes;

    GameObject resetButton;
    bool reset;

    Vector3 p0;
    Vector3 p1;
    Vector3 newp0;
    Vector3 newp1;

    bool textAdded = false;

    private float counter = 0;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        session = GameObject.Find("AR Session Origin");

        Scene currentScene = SceneManager.GetActiveScene();

        //gets the right cube list depending on the scene (different scenes have different scripts)
        if (currentScene.name == "Part1")
        {
            cubes = session.GetComponent<SceneController>().cubes;
        }
        else
        {
            cubes = session.GetComponent<SceneController2>().cubes;
        }
        

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.015f;
        lineRenderer.endWidth = 0.015f;

        p0 = cubes[cubes.Count - 2].transform.position;
        p1 = cubes[cubes.Count - 1].transform.position;

        lineRenderer.SetPosition(0, p0);
        lineRenderer.SetPosition(1, p0);
    }

    // Update is called once per frame
    void Update()
    {
        newp0 = lineRenderer.GetPosition(1);
        newp1 = Vector3.Lerp(newp0, p1, 6.0f * Time.deltaTime);
        lineRenderer.SetPosition(1, newp1);

        if (lineRenderer.GetPosition(1) == p1)
        {
            if (!textAdded)
            {
                distanceTextPrefab.GetComponent<TextMesh>().text = Vector3.Distance(p0, p1).ToString("0.00");
                distanceText = Instantiate(distanceTextPrefab, ((p0 + p1) / 2) + new Vector3(0, 0.02f, 0), Quaternion.identity);
                textAdded = true;
            }
        }
        

    }
}
