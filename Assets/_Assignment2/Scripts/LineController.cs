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
    Vector3 newp1;
    Vector3 offset = new Vector3(0, 0.035F, 0);

    bool textAdded = false;

    private float counter = 0;
    private float distance;

    public float lineDrawSpeed = 0.008F;

    // Start is called before the first frame update
    void Start()
    {
        session = GameObject.Find("AR Session Origin");
        cubes = session.GetComponent<SceneController>().cubes;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.015F;
        lineRenderer.endWidth = 0.015F;

        p0 = cubes[cubes.Count - 2].transform.position + offset;
        p1 = cubes[cubes.Count - 1].transform.position + offset;

        lineRenderer.SetPosition(0, p0);
        lineRenderer.SetPosition(1, p0);
    }

    // Update is called once per frame
    void Update()
    {
        newp1 = Vector3.Lerp(lineRenderer.GetPosition(1), p1, lineDrawSpeed * Time.deltaTime);
        lineRenderer.SetPosition(1, newp1);

        if (lineRenderer.GetPosition(1) == p1)
        {
            if (!textAdded)
            {
                distanceTextPrefab.GetComponent<TextMesh>().text = Vector3.Distance(p0, p1).ToString("0.00");
                distanceText = Instantiate(distanceTextPrefab, ((p0 + p1) / 2) + offset, Quaternion.identity);
                textAdded = true;
            }
        }
        

    }
}
