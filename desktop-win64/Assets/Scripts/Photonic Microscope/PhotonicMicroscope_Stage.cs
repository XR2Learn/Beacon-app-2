using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PhotonicMicroscope_Stage : InteractiveObject {

    public GameObject ControlMicroscopingUI;

    public const float MaxWidth = -0.119F;
    public const float MinWidth = -0.173F;

    [HideInInspector]
    public float Width;

    public const float dy = 0.001F;


    void setWidth (float _Width) {
        Width = _Width;
        transform.localPosition = new Vector3 (_Width, transform.localPosition.y, transform.localPosition.z);
    }

    // Start is called before the first frame update
    public override void Start () {

        setWidth (-0.1636F);

    }

    public void move (int _direction) {
        setWidth (Width + _direction * dy);
        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().moveImage (Axes.Y_Axis, _direction);
    }

    public void setImage_for_Specimen (GameObject _SpecimenOnHolder) {

        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().setImage_for_Specimen (_SpecimenOnHolder);

    }

}
