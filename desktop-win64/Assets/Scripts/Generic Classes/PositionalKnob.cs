using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalKnob : Knob {

    [HideInInspector]
    public int MaxPosition;
    [HideInInspector]
    public int MinPosition;
    [HideInInspector]
    public int Position;

    [HideInInspector]
    public int Step;

    // Start is called before the first frame update
    public override void Start () {
    }

    public override void rotate (int _direction) {

        if (_direction > 0 && Position + Step <= MaxPosition) {

            turn (da);

            Position += Step;

            affectOtherComponents (Position);

        } else if (_direction < 0 && Position - Step >= MinPosition) {

            turn (-da);

            Position -= Step;

            affectOtherComponents (Position);

        }


    }

}
