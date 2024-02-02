using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

    public const bool ON = true;
    public const bool OFF = false;

    public GameObject ControlLightIntensityKnob;
    public GameObject ControlUI;

    int LightIntensity;    
    bool InstrumentState;

    [HideInInspector]
    public float Min_ON_Brightness;
    [HideInInspector]
    public float Max_ON_Brightness;
    [HideInInspector]
    public float OFF_Brightness;

    [HideInInspector]
    public float Brightness;

    float Max_LightIntensity;
    float Min_LightIntensity;

    float Slope;
    float Constant;

    // Start is called before the first frame update
    public virtual void Start () {

        InstrumentState = OFF;

        Max_LightIntensity = ControlLightIntensityKnob.GetComponent<LightIntensityKnob> ().MaxPosition;
        Min_LightIntensity = ControlLightIntensityKnob.GetComponent<LightIntensityKnob> ().MinPosition;

        Slope = (Max_ON_Brightness - Min_ON_Brightness) / (Max_LightIntensity - Min_LightIntensity);
        Constant = Min_ON_Brightness - Slope * Min_LightIntensity;

        LightIntensity = ControlLightIntensityKnob.GetComponent<LightIntensityKnob> ().MinPosition;

    }

    public void updateState (bool _InstrumentState) {
        InstrumentState = _InstrumentState;
        updateBrightness ();
    }

    public void updateLightIntensity (int _LightIntensity) {
        LightIntensity = _LightIntensity;
        updateBrightness ();
    }

    public virtual void updateBrightness () {

        if (InstrumentState == OFF)
            Brightness = OFF_Brightness;
        else
            Brightness = Slope * LightIntensity + Constant;

        GetComponent<Renderer> ().material.color = new Color (Brightness, Brightness, Brightness, 1F);

        if (ControlUI != null)
            ControlUI.GetComponent<ExtraUI> ().updateLightBrightness (Brightness);

    }



}
