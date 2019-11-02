using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GameObject.Find("AR Camera").transform);
        transform.Rotate(new Vector3(0, 180, 0)); //rotate bc its backwards otherwise

        if (GameObject.Find("ResetButton").GetComponent<buttonController>().resetCube)
        {
            Destroy(transform.gameObject);
        }


    }
}
