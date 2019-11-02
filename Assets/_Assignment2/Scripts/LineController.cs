using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LineController : MonoBehaviour
{
    LineRenderer lineRenderer;

    GameObject session;

    public GameObject distanceTextPrefab;
    public GameObject distanceText;
    
    List<GameObject> cubes;


    Vector3 p0;
    Vector3 p1;

    Vector3 newp0;
    Vector3 newp1;

    Vector3 lineCenter;

    bool textAdded = false;

    float distance;

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

        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        p0 = cubes[cubes.Count - 2].transform.position;
        p1 = cubes[cubes.Count - 1].transform.position;

        distance = Vector3.Distance(p0, p1);
        lineCenter = (p0 + p1) / 2;

        lineRenderer.SetPosition(0, p0);
        lineRenderer.SetPosition(1, p0);
    }

    // Update is called once per frame
    void Update()
    {
        newp0 = lineRenderer.GetPosition(1);
        newp1 = Vector3.Lerp(newp0, p1, 6.0f * Time.deltaTime);
        lineRenderer.SetPosition(1, newp1);

        if (lineRenderer.GetPosition(1) == p1 && !textAdded)
        {
                distanceTextPrefab.GetComponent<TextMesh>().text = distance.ToString("0.00");
                distanceText = Instantiate(distanceTextPrefab, lineCenter + new Vector3(0, 0.02f, 0), Quaternion.identity);
                textAdded = true;
        }
    }
}
