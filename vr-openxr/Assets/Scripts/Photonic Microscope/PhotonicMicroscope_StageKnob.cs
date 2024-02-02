using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_StageKnob : Knob {

    public GameObject ControlStage;

    // Start is called before the first frame update
    public override void Start () {

        PivotAxis = Axes.X_Axis;
        RotationAxis = Axes.Y_Axis;

        da = 4F;

    }

    public override void rotate (int _direction) {

        if (_direction > 0F && ControlStage.GetComponent<PhotonicMicroscope_Stage> ().Width < PhotonicMicroscope_Stage.MaxWidth) {

            transform.Rotate (0F, da, 0F);

            ControlStage.GetComponent<PhotonicMicroscope_Stage> ().move (+1);

        } else if (_direction < 0F && ControlStage.GetComponent<PhotonicMicroscope_Stage> ().Width > PhotonicMicroscope_Stage.MinWidth) {

            transform.Rotate (0F, -da, 0F);

            ControlStage.GetComponent<PhotonicMicroscope_Stage> ().move (-1);

        }

    }

}

