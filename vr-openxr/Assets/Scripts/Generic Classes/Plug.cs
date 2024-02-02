using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : InteractiveObject {

    public GameObject ControlCable;

    // Start is called before the first frame update
    public override void Start() {        
    }

    public override bool IsRestrained () {
        return true;
    }

}
