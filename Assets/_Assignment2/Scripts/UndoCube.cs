using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UndoCube : MonoBehaviour
{
    public bool undo;
    // Start is called before the first frame update

    public void setUndoTrue()
    {
        undo = true;
    }

    public void setUndoFalse()
    {
        undo = false;
    }

}
