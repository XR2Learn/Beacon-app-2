using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : InteractiveObject {

    [HideInInspector]
    public Vector3 UprightRotation;

    // Start is called before the first frame update
    public override void Start () {
        UprightRotation = transform.localEulerAngles;
    }

    public virtual void restoreUprightRotation () {
        transform.localEulerAngles = UprightRotation;
    }

}
