using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedoCube : MonoBehaviour
{
    public bool redo;
    // Start is called before the first frame update

    public void setRedoTrue()
    {
        redo = true;
    }

    public void setRedoFalse()
    {
        redo = false;
    }

}
