using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingKnob : Knob {

    // Start is called before the first frame update
    public override void Start () {
    }


    public override void rotate (int _direction) {

        turn (_direction * da);

        affectOtherComponents ();

    }

}
