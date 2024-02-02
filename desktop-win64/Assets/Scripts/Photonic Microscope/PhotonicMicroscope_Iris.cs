using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_Iris : MonoBehaviour {

    public GameObject ControlApertureKnob;
    public GameObject ControlMicroscopingUI;

    float Diameter;

    float ApertureKnob_MaxPosition;

    public void setDiameter (int _ApertureKnob_Position) {
        
        Diameter = (float)_ApertureKnob_Position / ApertureKnob_MaxPosition;

        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().updateIrisDiameter (Diameter);

    }

    // Start is called before the first frame update
    void Start () {

        ApertureKnob_MaxPosition = 
            ControlApertureKnob.GetComponent<PhotonicMicroscope_ApertureKnob> ().MaxPosition;

        Diameter = 0F;

    }




}
