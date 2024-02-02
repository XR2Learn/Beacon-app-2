using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Drawer : PositionalKnob, IDrawer {

    // Start is called before the first frame update
    public override void Start () {

        PivotAxis = Axes.Y_Axis;

        MinPosition = 0;
        MaxPosition = 120;
        Step = 5;

        da = -0.02F; //in this class, it represents z-axis offset, not angle

        Position = MinPosition;

    }

    public override void turn (float _Angle) {
        float Offset = _Angle;
        move (Offset);
    }

    public void move (float _Offset) {
        transform.Translate (0F, 0F, _Offset);
    }

}
