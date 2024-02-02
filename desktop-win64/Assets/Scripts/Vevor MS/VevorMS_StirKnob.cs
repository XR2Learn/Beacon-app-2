using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VevorMS_StirKnob : PositionalKnob {

    public TextMeshPro RPMIndication;

    const int Multiplier = 40;

    // Start is called before the first frame update
    public override void Start () {
        
        PivotAxis = Axes.X_Axis;
        RotationAxis = Axes.X_Axis;
        
        MinPosition = 0;
        MaxPosition = 70;
        Step = 1;

        da = -3F;

        Position = MinPosition;

    }

    public override void affectOtherComponents (int _NewPosition) {
        RPMIndication.text = (_NewPosition * Multiplier).ToString ();
    }

}
