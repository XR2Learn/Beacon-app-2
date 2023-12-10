using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityKnob : PositionalKnob {

    public GameObject ControlLamp;

    // Start is called before the first frame update
    public override void Start () {
    }

    public override void affectOtherComponents (int _NewPosition) {
        ControlLamp.GetComponent<Lamp> ().updateLightIntensity (_NewPosition);
    }

}
