using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VevorMS_Switch : Switch {

    // Start is called before the first frame update
    public override void Start () {

        base.Start ();

        RotationAxis = Axes.Z_Axis;

        Angle_for_Off = 38F;
        Angle_for_On = 0F;

    }

}
