using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class resetScene : MonoBehaviour
{
    public bool resetCube;

    public void setResetTrue()
    {
        resetCube = true;
    }

    public void setResetFalse()
    {
        resetCube = false;
    }

}
