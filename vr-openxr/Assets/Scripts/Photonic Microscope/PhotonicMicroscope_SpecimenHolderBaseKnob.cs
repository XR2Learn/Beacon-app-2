using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_SpecimenHolderBaseKnob : Knob {

    public GameObject ControlSpecimenHolderBase;

    // Start is called before the first frame update
    public override void Start () {

        PivotAxis = Axes.X_Axis;
        RotationAxis = Axes.Z_Axis;

        da = 4F;

    }

    public override void rotate (int _direction) {

        if (_direction > 0F && ControlSpecimenHolderBase.GetComponent<PhotonicMicroscope_SpecimenHolderBase> ().Length < PhotonicMicroscope_SpecimenHolderBase.MaxLength) {

            transform.Rotate (0F, da, 0F);

            ControlSpecimenHolderBase.GetComponent<PhotonicMicroscope_SpecimenHolderBase> ().move (+1);

        } else if (_direction < 0F
          && ControlSpecimenHolderBase.GetComponent<PhotonicMicroscope_SpecimenHolderBase> ().Length > PhotonicMicroscope_SpecimenHolderBase.MinLength) {

            transform.Rotate (0F, -da, 0F);

            ControlSpecimenHolderBase.GetComponent<PhotonicMicroscope_SpecimenHolderBase> ().move (-1);

        }

    }

}
