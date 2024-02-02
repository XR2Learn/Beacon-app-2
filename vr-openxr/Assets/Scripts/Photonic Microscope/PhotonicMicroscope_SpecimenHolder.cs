using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonicMicroscope_SpecimenHolder : InteractiveObject {

    public GameObject ControlStage;
    public GameObject ControlClipController;

    GameObject SpecimenOn;



    void setAttachedSpecimen (GameObject _Specimen) {

        SpecimenOn = _Specimen;

        ControlStage.GetComponent<PhotonicMicroscope_Stage> ().setImage_for_Specimen (_Specimen);

    }


    // Start is called before the first frame update
    public override void Start () {

        SpecimenOn = null;

    }

    public override Values_After_JointUse use_with (GameObject _OtherObject) {

        Values_After_JointUse ResultValues = new Values_After_JointUse (false, null, false, null, false);

        if (SpecimenOn == null && _OtherObject.GetComponent<Slide> ()) {

            openClipController ();

            ResultValues = new Values_After_JointUse (true, gameObject, true, null, false);
            //it needs to be placed BEFORE placeIn because in placeIn, the specimen becomes child of the specimen holder, and changing an object's interactivity (here specimen holder's) affects the interactivity of its children

            placeIn (_OtherObject);

            setAttachedSpecimen (_OtherObject);
            
        }

        return ResultValues;

    }



    void openClipController () {

        ControlClipController.transform.localEulerAngles = new Vector3 (0F, 8.683F, 0F);

    }

    void closeClipController () {

        ControlClipController.transform.localEulerAngles = new Vector3 (0F, 0F, 0F);

    }

    void placeIn (GameObject _OtherObject) {

        _OtherObject.transform.parent = transform;
        _OtherObject.transform.localPosition = new Vector3 (-0.0226F, -0.0021F, 0.0062F);
        _OtherObject.transform.localEulerAngles = new Vector3 (0F, 0F, 0F);

    }

    public override void evacuate (GameObject _Object) {

        setAttachedSpecimen (null);

        _Object.GetComponent<InteractiveObject> ().Place = null;

        _Object.transform.parent = null;

        closeClipController ();

        GetComponent<MouseUI> ().setInteractivity (true);

    }

    public void setActivation_of_SpecimenMovement (bool _ActivationStatus) {

        if (SpecimenOn != null)
            SpecimenOn.GetComponent<BoxCollider> ().enabled = _ActivationStatus;
    
    }

}
