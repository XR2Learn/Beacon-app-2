using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Knob : InteractiveObject {

    [HideInInspector]
    public Axes PivotAxis; //Mouse movement axis - X or Y
    [HideInInspector]
    public Axes RotationAxis; //Mesh rotation axis
    
    [HideInInspector]
    public float da;


    [HideInInspector]
    public int u = 1; //because most knobs "open" clockwise


    // Start is called before the first frame update
    public override void Start () {
    }

    public override void pivot (Vector2 _dv) {

        int direction;

        if (PivotAxis == Axes.X_Axis)
            direction = (int) Mathf.Sign (_dv.x);
        else
            direction = (int) Mathf.Sign (_dv.y);

        direction *= u;

        if (direction != 0)
            rotate (direction);

    }

    public virtual void rotate (int _direction) {
    }


    public override void done_pivoting () {        
    }





    public virtual void turn (float _Angle) {

        if (RotationAxis == Axes.X_Axis)
            transform.Rotate (_Angle, 0F, 0F);
        else if (RotationAxis == Axes.Y_Axis)
            transform.Rotate (0F, _Angle, 0F);
        else
            transform.Rotate (0F, 0F, _Angle);

    }


    public virtual void affectOtherComponents () {
    }

    public virtual void affectOtherComponents (int _NewPosition) {
    }

}
