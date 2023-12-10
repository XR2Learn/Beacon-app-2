using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_StageBase : MonoBehaviour {

    public GameObject ControlCondenser;
    public GameObject ControlCondenserKnob;
    public GameObject ControlMicroscopingUI;

    public const float MaxHeight = 0.23F;
    public const float MinHeight = 0.1768353F;

    public const float HeightRange = MaxHeight - MinHeight;

    public const float HeightToAngleRatio = 0.00008F;

    [HideInInspector]
    public float Height;

    [HideInInspector]
    public float PerfectHeight;

    int Focus;

//  float Coefficient;


    public static readonly Dictionary<int, float> PerfectHeight_for_Focus = 
        new Dictionary<int, float> {
            {4, MinHeight + 0.403566185F * HeightRange},
            {10, MinHeight + 0.540452635F * HeightRange},
            {40, MinHeight + 0.633339868F * HeightRange},
            {100, MinHeight + 0.63822867F * HeightRange}
    };


    void setHeight (float _height) {

        Height = _height;

        transform.localPosition =
            new Vector3 (transform.localPosition.x, Height, transform.localPosition.z);

        float Factor = Mathf.Log (Mathf.Abs (Height - PerfectHeight) + 1,
            LogarithmicBase.ObjectiveLens_Focus[Focus]);

        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().updateStageBaseBlurring (Factor);

    }


    public void Restore_to_OriginalHeight () {
        setHeight (MinHeight);
    }

    // Start is called before the first frame update
    void Start () {
        setHeight (MinHeight);
    }

    public void move (float _da, float _Multiplier) {

        float dh = _Multiplier * _da * HeightToAngleRatio;

        float increment;

        if (dh > 0F && Height + dh > MaxHeight) {
            increment = MaxHeight - Height;
            setHeight (MaxHeight);
        } else if (dh < 0F && Height + dh < MinHeight) {
            increment = MinHeight - Height;
            setHeight (MinHeight);
        } else {
            increment = dh;
            setHeight (Height + dh);
        }

        if (_da < 0F &&
            ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().Height < PhotonicMicroscope_Condenser.OriginalMinHeight &&
            ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().Height < ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().MinHeight)
            ControlCondenserKnob.GetComponent<PhotonicMicroscope_CondenserKnob> ().rotate (_da);

        ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().MinHeight -= increment;

    }

    public void updateFocus (int _Focus) {
        Focus = _Focus;
        update_PerfectHeight ();
    }

    void update_PerfectHeight () {

        PerfectHeight = PerfectHeight_for_Focus[Focus];

        setHeight (Height); //necessary, for the redefinition of Factor in microscoping

    }

}
