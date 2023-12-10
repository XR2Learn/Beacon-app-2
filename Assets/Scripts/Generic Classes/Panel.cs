using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Panel : InteractiveObject {

    public GameObject ControlInstrument;

    // Start is called before the first frame update
    public override void Start () {
    }

    public override void zoom () {        
        ControlInstrument.GetComponent<Instrument> (). toggleView ();
    }

    public virtual void updateZoomability (bool _NewZoomability) {
        GetComponent<MouseUI> ().Zoomable = _NewZoomability;
    }

}
