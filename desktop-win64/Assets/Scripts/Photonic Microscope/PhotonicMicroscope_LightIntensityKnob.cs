using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_LightIntensityKnob : LightIntensityKnob {

    // Start is called before the first frame update
    public override void Start () {

        PivotAxis = Axes.X_Axis;
        RotationAxis = Axes.Y_Axis;

        MinPosition = 1;
        MaxPosition = 24;
        Step = 1;

        da = -12F;

        Position = 1;

    }

}
