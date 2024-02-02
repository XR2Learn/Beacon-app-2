using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_Condenser : MonoBehaviour {

    public GameObject ControlMicroscopingUI;

    public const float OriginalMinHeight = -0.0017F;

    public const float MaxHeight = -0.0011F;

    [HideInInspector]
    public float MinHeight;

    const float LogBase = LogarithmicBase.CondenserLens;

    [HideInInspector]
    public float Height;

//  const float UltimateMinHeight = -0.05486423F;

    public const float PerfectHeight = MaxHeight;

    public const float HeightToAngleRatio = 0.0002F;

    void setHeight (float _Height) {
        
        Height = _Height;

        transform.localPosition = new Vector3 (transform.localPosition.x, Height, transform.localPosition.z);

        float Factor = Mathf.Log (Mathf.Abs (Height - PerfectHeight) + 1, LogBase);

        ControlMicroscopingUI.GetComponent<PhotonicMicroscope_MicroscopingUI> ().updateCondenserBlurring (Factor);

    }

    // Start is called before the first frame update
    void Start () {

        MinHeight = OriginalMinHeight;
        
        setHeight (MinHeight);

    }

    public void move (float _da) {

        float dh = _da * HeightToAngleRatio;

        if (dh > 0F && Height + dh > MaxHeight)
            setHeight (MaxHeight);
        else if (dh < 0F && Height + dh < MinHeight)
            setHeight (MinHeight);
        else
            setHeight (Height + dh);

    }

}
