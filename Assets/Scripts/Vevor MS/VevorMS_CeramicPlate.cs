using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VevorMS_CeramicPlate : InteractiveObject {
        
    // Start is called before the first frame update
    public override void Start () {

    }

    public override Values_After_JointUse use_with (GameObject _OtherObject) {

        Values_After_JointUse ResultValues = new Values_After_JointUse (false, null, false, null, false);

        if (_OtherObject.GetComponent<Vessel> ()) {

            ResultValues = new Values_After_JointUse (true, gameObject, true, null, false);

            placeOver (_OtherObject);


        }

        return ResultValues;

    }


    void placeOver (GameObject _OtherObject) {

        _OtherObject.transform.parent = gameObject.transform;
        _OtherObject.transform.localPosition = new Vector3 (0F, 0.0252F, 0F);
        _OtherObject.transform.localEulerAngles = new Vector3 (0F, 107F, 0F);

    }

}
