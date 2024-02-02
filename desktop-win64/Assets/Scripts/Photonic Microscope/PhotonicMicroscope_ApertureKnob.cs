using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_ApertureKnob : PositionalKnob {

    public GameObject ControlIris;

    // Start is called before the first frame update
    public override void Start () {

        PivotAxis = Axes.X_Axis;
        RotationAxis = Axes.Y_Axis;

        MinPosition = 0;
        MaxPosition = 40;

        Step = 1;

        da = 2.114F;

        u = -1; //because it opens counter-clockwise

    }

    public override void affectOtherComponents (int _NewPosition) {
        ControlIris.GetComponent<PhotonicMicroscope_Iris> ().setDiameter (_NewPosition);
    }

}
