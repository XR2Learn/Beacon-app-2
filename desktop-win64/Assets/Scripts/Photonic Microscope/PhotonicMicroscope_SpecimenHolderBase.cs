using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PhotonicMicroscope_SpecimenHolderBase : MonoBehaviour {

    public GameObject ControlMicroscopingUI;

    public const float MaxLength = 0.046F;
    public const float MinLength = -0.014F;

    [HideInInspector]
    public float Length;

    public const float dx = 0.001F;

    void setLength (float _Length) {

        Length = _Length;
        transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, _Length);

    }

    // Start is called before the first frame update
    void Start () {
        setLength (0F);
    }

    public void move (int _direction) {

        setLength (Length + _direction * dx);

        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().moveImage (Axes.X_Axis, _direction);

    }

}
