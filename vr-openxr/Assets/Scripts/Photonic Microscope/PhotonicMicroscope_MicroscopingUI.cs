using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Unity.Collections.LowLevel.Unsafe;

using UnityEngine;
using UnityEngine.UI;

public class LogarithmicBase {

    public const float ObjectiveLens_4X = 1.01F;
    public const float ObjectiveLens_10X = 1.005F;
    public const float ObjectiveLens_40X = 1.0001F;
    public const float ObjectiveLens_100X = 1.00005F;
    public const float CondenserLens = 1.00005F;
    public const float OcularLens = 1.005F;

    public static readonly Dictionary<int, float> ObjectiveLens_Focus = new Dictionary<int, float> {
        {4, ObjectiveLens_4X},
        {10, ObjectiveLens_10X},
        {40, ObjectiveLens_40X},
        {100, ObjectiveLens_100X}
    };

}


public class PhotonicMicroscope_MicroscopingUI: ExtraUI {

    private const int Speed_X = 14;
    private const int Speed_Y = 16;

    public GameObject ControlLightFG;
    public GameObject ControlIrisFG;

    public GameObject ControlWhiteBG;

    public GameObject ImageClearFG;
    public GameObject ImageBlurryFG;

    const float LightFG_Opacity_for_Min_ON_Brightness = 0.94F;
    const float LightFG_Opacity_for_Max_ON_Brightness = 0F;

    float Slope;
    float Constant;

    float Lamp_OFF_Brightness;

    int Focus;

    int OldFocus;

    int X;
    int Y;

    float LeftOcularBlurring;
    float RightOcularBlurring;

    float CondenserBlurring;
    float StageBaseBlurring;

    float OverallBlurring;


    public override void Start() {

        Lamp_OFF_Brightness = ControlLamp.GetComponent<PhotonicMicroscope_Lamp>().OFF_Brightness;

        Slope = (LightFG_Opacity_for_Max_ON_Brightness - LightFG_Opacity_for_Min_ON_Brightness)
            / (ControlLamp.GetComponent<PhotonicMicroscope_Lamp>().Max_ON_Brightness -
            ControlLamp.GetComponent<PhotonicMicroscope_Lamp>().Min_ON_Brightness);

        Constant = LightFG_Opacity_for_Max_ON_Brightness -
            Slope * ControlLamp.GetComponent<PhotonicMicroscope_Lamp>().Max_ON_Brightness;

        X = 0;
        Y = 0;

        //OldFocus = 4; //just for initilization purposes
        //Focus = OldFocus;

        setImage_for_Specimen(null);

    }

    public void setImage_for_Specimen(GameObject _Specimen) {

        if (_Specimen != null) {

            //_Specimen can only be Slide
            ImageClearFG.GetComponent<Image>().sprite = _Specimen.GetComponent<Slide>().ImageClear;
            ImageBlurryFG.GetComponent<Image>().sprite = _Specimen.GetComponent<Slide>().ImageBlurry;

        }
        else {

            ImageClearFG.GetComponent<Image>().sprite = null;
            ImageBlurryFG.GetComponent<Image>().sprite = null;

        }

    }


    public void updateFocus(int _NewFocus) {

        if (Focus == 0)
            OldFocus = 4; //the function is also called from revolving nosepiece in the begging of the game, so we need to give OldFocus a random non-zero value so that we won't have division by zero later on
        else
            OldFocus = Focus;

        Focus = _NewFocus;

        float NewScale = 0.125F * Focus; // = (focus / 4 ) * 1/2
        ImageClearFG.transform.localScale = new Vector3(NewScale, NewScale);

        X = X * Focus / OldFocus;
        Y = Y * Focus / OldFocus;

        ImageClearFG.transform.localPosition = new Vector3(X, Y);

        updateOverallBlurring();

        //		print (ImageClear.transform.localPosition);

    }


    public void moveImage(Axes _axis, int _direction) {

        if (_axis == Axes.X_Axis)
            X += _direction * Focus * Speed_X;
        else //if (_axis == Axes.Y_Axis)
            Y += _direction * Focus * Speed_Y;

        ImageClearFG.transform.localPosition = new Vector3(X, Y);

        //		print (ImageClear.transform.localPosition);

    }

    public void setVisibility_of_SpecimenImage(bool _Visibility) {
        ControlWhiteBG.SetActive(_Visibility);
    }


    public override void updateLightBrightness(float _LightBrightness) {
        ControlLightFG.GetComponent<Image>().color = new Color(1F, 1F, 1F, LightFG_Opacity(_LightBrightness));
    }


    float LightFG_Opacity(float _Brightness) {

        if (_Brightness == Lamp_OFF_Brightness)
            return 1F;
        else
            return Slope * _Brightness + Constant;

    }


    public void updateIrisDiameter(float _IrisDiameter) {
        ControlIrisFG.GetComponent<Image>().color = new Color(0F, 0F, 0F, 1F - _IrisDiameter);
    }


    public void updateCondenserBlurring(float _Factor) {

        CondenserBlurring = _Factor;

        //      print ("CondenserBlurring = " + CondenserBlurring);

        updateOverallBlurring();

    }


    public void updateStageBaseBlurring(float _Factor) {

        StageBaseBlurring = _Factor;

        //      print ("StageBase Blurring = " + StageBaseBlurring);

        updateOverallBlurring();

    }


    public void updateOcularBlurring(OcularPosition _Position, float _Factor) {

        Debug.Log($"[PhotonicMicroscope_MicroscopingUI] updateOcularBlurring, factor: {_Factor}");

        if (_Position == OcularPosition.Left)
            LeftOcularBlurring = _Factor;
        else //if (_Position == OcularPosition.Right)
            RightOcularBlurring = _Factor;

        //      print ("Left Ocular Blurring = " + LeftOcularBlurring);
        //      print ("Right Ocular Blurring = " + RightOcularBlurring);

        updateOverallBlurring();

    }


    void updateOverallBlurring() {

        OverallBlurring = 0.05F * (2.5F * LeftOcularBlurring + 2.5F * RightOcularBlurring + 8.5F * StageBaseBlurring + 6.5F * CondenserBlurring);
        // 2.5 + 2.5 + 8.5 + 6.5 = 20; 1/20 = 0.05

        OverallBlurring = Mathf.Min(OverallBlurring, 1F);

        //      print ("Overall Blurring = " + OverallBlurring);

        ImageBlurryFG.GetComponent<Image>().color = new Color(1F, 1F, 1F, OverallBlurring);

    }

}
