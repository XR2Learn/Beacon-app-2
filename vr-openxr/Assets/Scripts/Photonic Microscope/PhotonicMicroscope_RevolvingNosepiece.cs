using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_RevolvingNosepiece : RevolvingKnob {

    public GameObject ControlStageBase;
    public GameObject ControlMicroscopingUI;

    [HideInInspector]
    public int Focus;

    void setFocus (int _Focus) {
        Focus = _Focus;
        ControlStageBase.GetComponent<PhotonicMicroscope_StageBase> ().updateFocus (Focus);
        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().updateFocus (Focus);
    }


    // Start is called before the first frame update
    public override void Start () {

        PivotAxis = Axes.X_Axis;
        RotationAxis = Axes.Y_Axis;

        da = -6F;

        setFocus (10);

    }


    public override void affectOtherComponents () {
        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().setVisibility_of_SpecimenImage (false);
    }



    public override void done_pivoting () {

        float Angle = transform.localRotation.eulerAngles.y;

        Angle = EquivalentPositiveAngle_of (Angle);

        float NearestAngle= -1000F; //just a symbolic value; not to be used later on

        if (Angle > 0F && Angle < 90F) {

            if (Mathf.Abs (Angle - 0F) < Mathf.Abs (Angle - 90F)) {

                NearestAngle = 0F;

                setFocus (4);

            } else {

                NearestAngle = 90F;

                setFocus (10);

            }
        } else if (Angle > 90F && Angle < 180F) {

            if (Mathf.Abs (Angle - 90F) < Mathf.Abs (Angle - 180F)) {

                NearestAngle = 90F;

                setFocus (10);

            } else {

                NearestAngle = 180F;

                setFocus (40);

            }
        } else if (Angle > 180F && Angle < 270F) {

            if (Mathf.Abs (Angle - 180F) < Mathf.Abs (Angle - 270F)) {

                NearestAngle = 180F;

                setFocus (40);

            } else {

                NearestAngle = 270F;

                setFocus (100);

            }
        } else if (Angle > 270F && Angle < 360F) {

            if (Mathf.Abs (Angle - 270F) < Mathf.Abs (Angle - 360F)) {

                NearestAngle= 270F;

                setFocus (100);

            } else {

                NearestAngle= 360F;

                setFocus (4);

            }
        
        }

        transform.Rotate (0F, NearestAngle - Angle, 0F);

        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> (). setVisibility_of_SpecimenImage (true);

    }


    float EquivalentPositiveAngle_of (float _Angle) {

        if (_Angle < 0F)
            _Angle += 360F;

        return _Angle;

    }

}
