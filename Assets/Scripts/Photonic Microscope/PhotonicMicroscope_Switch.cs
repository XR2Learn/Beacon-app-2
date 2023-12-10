using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_Switch : Switch {

    // Start is called before the first frame update
    public override void Start () {

        base.Start ();

        RotationAxis = Axes.Y_Axis;
        Angle_for_Off = -16F;
        Angle_for_On = 16F;

    }

}
