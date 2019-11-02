using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonController : MonoBehaviour
{
    public bool undo;
    public bool redo;

    public bool resetCube;
    public bool placeCube;

    public void setUndoTrue()
    {
        undo = true;
    }

    public void setUndoFalse()
    {
        undo = false;
    }

    public void setRedoTrue()
    {
        undo = true;
    }

    public void setRedoFalse()
    {
        undo = false;
    }

    public void setResetTrue()
    {
        resetCube = true;
    }

    public void setResetFalse()
    {
        resetCube = false;
    }

    public void setPlaceTrue()
    {
        placeCube = true;
    }

    public void setPlaceFalse()
    {
        placeCube = false;
    }
}
