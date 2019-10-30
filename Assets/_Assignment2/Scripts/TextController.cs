using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    GameObject camera;
    GameObject resetButton;
    bool reset;

    Vector3 offset = new Vector3(0, 180, 0);
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("AR Camera");
        resetButton = GameObject.Find("ResetButton");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform);
        transform.Rotate(offset);

        reset = resetButton.GetComponent<resetScene>().resetCube;

        if (reset)
        {
            Destroy(transform.gameObject);
        }
    }
}
