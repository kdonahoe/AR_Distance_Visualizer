using UnityEngine;

public class buttonController : MonoBehaviour
{
    //booleans that are set by button presses
    public bool undo;
    public bool redo; //redo isn't in use

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
