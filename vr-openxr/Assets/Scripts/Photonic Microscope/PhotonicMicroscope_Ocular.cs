using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum OcularPosition {
    Left,
    Right
}


public class PhotonicMicroscope_Ocular: Knob {

    public GameObject ControlMicroscopingUI;

    public OcularPosition Position;

    const float MinHeight = 0F;
    const float MaxHeight = 0.003F;
    const float PerfectHeight = 0.0005F;

    const float LogBase = LogarithmicBase.OcularLens;

    float Height;

    const float Theta = 34.555F;

    const float dh = 0.0001F;

    void setHeight(float _Height) {

        Height = _Height;

        float Factor = Mathf.Log(Mathf.Abs(Height - PerfectHeight) + 1, LogBase);

        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI>().updateOcularBlurring(Position, Factor);
    }

    // Use this for initialization
    public override void Start() {

        base.Start();

        da = 6F;

        setHeight(MinHeight);

    }

    public override void rotate(int _direction) {

        if (_direction < 0F && Height + dh <= MaxHeight) {

            transform.Rotate(0F, -da, 0F);
            transform.Translate(0F, dh, 0F);

            setHeight(Height + dh);

        }
        else if (_direction > 0F && Height - dh >= MinHeight) {

            transform.Rotate(0F, da, 0F);
            transform.Translate(0F, -dh, 0F);

            setHeight(Height - dh);

        }

    }

}
