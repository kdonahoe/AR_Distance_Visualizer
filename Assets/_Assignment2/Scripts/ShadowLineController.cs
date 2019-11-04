using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShadowLineController : MonoBehaviour
{
    LineRenderer lineRenderer;

    GameObject session;

    List<GameObject> shadows;

    Vector3 p0;
    Vector3 p1;
    Vector3 newp0;
    Vector3 newp1;


    // Start is called before the first frame update
    void Start()
    {
        session = GameObject.Find("AR Session Origin");

        Scene currentScene = SceneManager.GetActiveScene();
        shadows = session.GetComponent<SceneController2>().cubeShadows;

        p0 = shadows[shadows.Count - 2].transform.position;
        p1 = shadows[shadows.Count - 1].transform.position;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.015f;
        lineRenderer.endWidth = 0.015f;
        lineRenderer.SetPosition(0, p0);
        lineRenderer.SetPosition(1, p0);
    }

    // Update is called once per frame
    void Update()
    {
        newp0 = lineRenderer.GetPosition(1);
        newp1 = Vector3.Lerp(newp0, p1, 6.0f * Time.deltaTime);
        lineRenderer.SetPosition(1, newp1);
    }
}
