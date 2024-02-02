using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axes {
    X_Axis,
    Y_Axis,
    Z_Axis
}


public class InteractiveObject : MonoBehaviour {

    public const bool ON = true;
    public const bool OFF = false;

    public const bool connected = true;
    public const bool disconnected = false;

    public GameObject Place {
        get; set;
    }

    // Start is called before the first frame update
    public virtual void Start () {        
    }

    public virtual void press () {
    }

    public virtual void zoom () {
    }

    public virtual void pivot (Vector2 _dv) {
    }

    public virtual void done_pivoting () {
    }

    public virtual Values_After_JointUse use_with (GameObject _OtherObject) {
        Values_After_JointUse ReturnValues = new Values_After_JointUse (false);
        return ReturnValues;
    }

    public virtual void evacuate (GameObject _object) {
    }

    public virtual bool IsRestrained () {
        return false;
    }

}
