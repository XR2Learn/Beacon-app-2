using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_OcularLens : Panel {

    public GameObject OtherLens;

    public override void updateZoomability (bool _NewZoomability) {
        base.updateZoomability (_NewZoomability);
        OtherLens.GetComponent<MouseUI> ().Zoomable = _NewZoomability;
    }

}
