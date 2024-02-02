using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ExtraUI : MonoBehaviour {

    public GameObject ControlLamp;
    public GameObject ControlCanvas;

    // Start is called before the first frame update
    public virtual void Start () {
    }

    public void setActivationStatus (bool _NewActivationStatus) {
        ControlCanvas.SetActive (_NewActivationStatus);
    }

    public virtual void updateLightBrightness (float _LightBrightness) {
    }

}
