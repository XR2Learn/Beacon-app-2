using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : InteractiveObject {

    public GameObject ControlInstrument;

    [HideInInspector]
    public Axes RotationAxis;
    [HideInInspector]
    public float Angle_for_On;
    [HideInInspector]
    public float Angle_for_Off;

    [HideInInspector]
    public bool State;


    // Start is called before the first frame update    
    public override void Start () {
        State = OFF;
    }

    public override void press () {
        setState (!State);
    }

    void setState (bool _NewState) {

        State = _NewState;

        flip (State ? Angle_for_On : Angle_for_Off);

        ControlInstrument.GetComponent<Instrument> ().updateFeedback_from_PowerController (State);

    }


    void flip (float _NewAngle) {

        if (RotationAxis == Axes.X_Axis)
            transform.localEulerAngles = new Vector3 (_NewAngle, transform.localEulerAngles.y,
                transform.localEulerAngles.z);
        else if (RotationAxis == Axes.Y_Axis)
            transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, _NewAngle,
                transform.localEulerAngles.z);
        else
            transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x,
                transform.localEulerAngles.y, _NewAngle);

    }

}
