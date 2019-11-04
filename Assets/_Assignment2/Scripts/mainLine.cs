using UnityEngine;

public class mainLine : MonoBehaviour
{
    LineRenderer lineRenderer;

    GameObject camera;
    GameObject origin;

    GameObject currentCube;

    Vector3 cubeVelocity;

    private Vector3[] bezier = new Vector3[50];

    Vector3 p0;
    Vector3 p1; //control point
    Vector3 p2;

    float t;

    // Start is called before the first frame update
    void Start()
    {
        origin = GameObject.Find("AR Session Origin");
        camera = GameObject.Find("AR Camera");

        currentCube = origin.GetComponent<SceneController2>().currentCube;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

        lineRenderer.startWidth = 0.006F;
        lineRenderer.endWidth = 0.006F;
        lineRenderer.positionCount = 50; //50 points
    }

    void Update()
    {
        //point from user
        p0 = camera.transform.position - new Vector3(0, 1, 0);
        //point to cube
        p2 = currentCube.transform.position;

        //control point in center of line, scaled by velocity
        cubeVelocity = origin.GetComponent<SceneController2>().cubeVelocity;
        p1 = ((p0 + p2) / 2) + (cubeVelocity * 0.4f);

        //calculates bezier points
        for (int i = 1; i < 51; i++)
        {
            t = i / 50.0f;
            bezier[i - 1] = ((1 - t) * (1 - t)) * p0 + ((2 * t) - (2 * t * t)) * p1 + (t * t) * p2;
        }
        lineRenderer.SetPositions(bezier);
    }

}
