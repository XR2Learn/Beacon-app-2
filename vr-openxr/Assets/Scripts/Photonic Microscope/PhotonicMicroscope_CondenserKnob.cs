using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_CondenserKnob : Knob {

    public GameObject ControlCondenser;

    // Start is called before the first frame update
    public override void Start () {

        //no need to define PivotAxis and RotationAxis, since pivot() function is being overriden

        da = 1F;

    }

    public override void pivot (Vector2 _dv) {
    
        float direction = Mathf.Sign (-_dv.y);

        //the if-clauses are put here because rotate is also called from StageBase
        if (direction > 0F && 
            ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().Height < PhotonicMicroscope_Condenser.MaxHeight)

            rotate (da);

        else if (direction < 0F && 
            ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().Height > ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().MinHeight)

            rotate (-da);
    
    }

    //this function is different from rotate (int _direction) in the parental class
    public void rotate (float _da) {

        transform.Rotate (0F, _da, 0F);
        
        ControlCondenser.GetComponent<PhotonicMicroscope_Condenser> ().move (_da);

    }

}
