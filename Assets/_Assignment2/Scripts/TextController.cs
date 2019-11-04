using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    void Update()
    {
        //text rotates
        transform.LookAt(GameObject.Find("AR Camera").transform);
        transform.Rotate(new Vector3(0, 180, 0)); //rotate bc its backwards otherwise
    }
}
