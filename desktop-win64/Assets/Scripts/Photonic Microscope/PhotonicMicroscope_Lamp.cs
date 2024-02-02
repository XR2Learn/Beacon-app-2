using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonicMicroscope_Lamp : Lamp {

    // Start is called before the first frame update
    public override void Start () {

        OFF_Brightness = 0.31F;
        Min_ON_Brightness = 0.48F;
        Max_ON_Brightness = 1F;

        base.Start ();
        
    }


}
