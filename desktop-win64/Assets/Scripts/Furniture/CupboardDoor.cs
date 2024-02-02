using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardDoor : PositionalKnob {

    public GameObject Cupboard;

    // Start is called before the first frame update
    public override void Start () {

        PivotAxis = Axes.X_Axis;
        RotationAxis = Axes.Y_Axis;

        MinPosition = 0;
        MaxPosition = 36;

        Step = 1;

        da = -(180F - 0F) / (MaxPosition - MinPosition);

        Position = MinPosition;

    }

}
