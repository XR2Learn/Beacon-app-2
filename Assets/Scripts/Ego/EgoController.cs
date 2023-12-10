using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public enum Modes {
    Navigation,
    Microscoping
}


public class EgoController : MonoBehaviour {

    const int FurnitureLayer = 11;

    public float LinearSpeed;
    public float AngularSpeed;
    public float ZoomSpeed;
    public float InspectionSpeed;

    const float MaximumField_of_View = 60F;
    const float MinimumField_of_View = 38F;

    const float MaximumXAngle = 90F;
    const float MinimumXAngle = -90F;

    public const float Height = 3.603F;

    [HideInInspector]
    public float rotate;

    [HideInInspector]
    public float move;

    [HideInInspector]
    public float zoom;

    [HideInInspector]
    public bool ZoomUsed;

    bool ControlIsDown;

    [HideInInspector]
    public bool InspectionDone;

    [HideInInspector]
    public Modes Mode;

    [HideInInspector]
    public GameObject InstrumentOnFocus;

    //public for Save
    [HideInInspector]
    public Vector3 Saved_Position;
    
    //public for Save
    [HideInInspector]
    public Vector3 Saved_EulerAngles;

    //public for Save
    [HideInInspector]
    public float Saved_FieldOfView;

    bool Halt;


    // Start is called before the first frame update
    void Start () {
        
        ZoomUsed = false;

        ControlIsDown = false;
        InspectionDone = false;

        Mode = Modes.Navigation;

        Halt = false;

    }


    public void setMode (Modes _Mode) { //for navigation

        InstrumentOnFocus = null;
        Mode = _Mode;

        restoreSavedPresence ();

    }



    public void setMode (Modes _Mode, GameObject _InstrumentOnFocus, float _X_position, 
        float _Z_position, float _Y_angle, float _X_camera_angle, float _FieldOfView) {

        if (Mode == Modes.Navigation)
            saveCurrentPresence ();

        InstrumentOnFocus= _InstrumentOnFocus;
        Mode = _Mode;

        transform.localPosition = new Vector3 (_X_position,
            transform.localPosition.y, _Z_position);
        transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x,
            _Y_angle, transform.localEulerAngles.z);
        transform.localEulerAngles = new Vector3 (_X_camera_angle, transform.localEulerAngles.y, 
            transform.localEulerAngles.z);
        GetComponent<Camera> ().fieldOfView = _FieldOfView;

    }


    void saveCurrentPresence () {
        Saved_Position = transform.localPosition;
        Saved_EulerAngles = transform.localEulerAngles;
        Saved_FieldOfView = GetComponent<Camera> ().fieldOfView;
    }



    void restoreSavedPresence () {
        transform.localPosition = Saved_Position;
        transform.localEulerAngles = Saved_EulerAngles;
        GetComponent<Camera> ().fieldOfView = Saved_FieldOfView;
    }


    void OnGUI () {
        MouseUI.callOnGUI ();
    }


    // Update is called once per frame
    void Update () {

        if (Mode == Modes.Navigation) {

            rotate = Input.GetAxis ("Horizontal");

            move = Input.GetAxis ("Vertical");

            zoom = Input.GetAxis ("Mouse ScrollWheel");

        }

        if ((rotate != 0F || move != 0F) && !ControlIsDown && (ZoomUsed || InspectionDone)) {

            RestoreUprightPosition ();

            if (ZoomUsed) {

                GetComponent<Camera> ().fieldOfView = MaximumField_of_View;

                ZoomUsed = false;

            } else if (InspectionDone)
                InspectionDone = false;

        }

        if (rotate != 0F) {
            transform.Rotate (0F, AngularSpeed * rotate * Time.deltaTime, 0F);
            rotate = 0F;
        }

        if (move != 0F) {
            transform.Translate (0F, 0F, LinearSpeed * move * Time.deltaTime);
            move = 0F;
        }

        if (zoom != 0F
            && GetComponent<Camera> ().fieldOfView - zoom * ZoomSpeed <= MaximumField_of_View
            && GetComponent<Camera> ().fieldOfView - zoom * ZoomSpeed >= MinimumField_of_View) {

            if (InspectionDone) {

                RestoreUprightPosition ();

                GetComponent<Camera> ().fieldOfView = MaximumField_of_View;

                InspectionDone = false;

            }

            GetComponent<Camera> ().fieldOfView -= zoom * ZoomSpeed;

            transform.Rotate (zoom * ZoomSpeed, 0F, 0F);

            zoom = 0F;

            ZoomUsed = true;

        }



        if (Input.GetKeyDown (KeyCode.LeftControl) || Input.GetKeyDown (KeyCode.RightControl)) {
            
            ControlIsDown = true;

            if (ZoomUsed) {

                RestoreUprightPosition ();

                ZoomUsed = false;

            }

        }

        if (ControlIsDown) {

            float dy = MouseUI.occurence.delta.y;

            if (Mathf.Abs (dy) > 0F &&
                reduced (transform.localEulerAngles.x) + InspectionSpeed * (dy - 1) >= MinimumXAngle &&
                reduced (transform.localEulerAngles.x) + InspectionSpeed * (dy + 1) <= MaximumXAngle) {

                transform.Rotate (InspectionSpeed * dy, 0F, 0F);

                InspectionDone = true;

            }

        }

        if (Input.GetKeyUp (KeyCode.LeftControl) || Input.GetKeyUp (KeyCode.RightControl))
            ControlIsDown = false;

        if (Input.GetKeyUp (KeyCode.Escape))
            Application.Quit ();

    }


    void RestoreUprightPosition () {

        //getting back to upright position after having bent for zooming

        Quaternion rotation_origin = Quaternion.Euler (transform.localEulerAngles);
        Quaternion rotation_destination = Quaternion.Euler (0F, transform.localEulerAngles.y, 0F);

        Vector3 position_origin = transform.localPosition;
        Vector3 position_destination = new Vector3 (transform.localPosition.x, Height, transform.localPosition.z);

        transform.localRotation = Quaternion.Slerp (rotation_origin, rotation_destination, Time.maximumDeltaTime);
        transform.localEulerAngles = new Vector3 (0F, transform.localEulerAngles.y, 0F);

        transform.localPosition = Vector3.Lerp (position_origin, position_destination, Time.maximumDeltaTime);
        transform.localPosition = new Vector3 (transform.localPosition.x, Height, transform.localPosition.z);

    }



    float reduced (float _Angle) {

        float NewAngle = _Angle;

        if (NewAngle > 270F)
            NewAngle -= 360F;

        return NewAngle;

    }


    void OnCollisionEnter (Collision _collision) {

        if (_collision.gameObject.layer == FurnitureLayer) {
            
            if (!Halt) {
                move = 0F;
                Halt = true;
                StartCoroutine (unHalting ());
            }

        }

    }

    void OnCollisionExit (Collision _collision) {

        if (_collision.gameObject.layer == FurnitureLayer) {
        
            if (Halt) {
                move = 0F;
                Halt = false;
            }

        }
    }

    IEnumerator unHalting () {

        yield return new WaitUntil (() => Input.GetKeyUp (KeyCode.DownArrow)
        || Input.GetKeyUp (KeyCode.UpArrow));

        //transform.Translate (0.0f, 0.0f, -LinearSpeed * Time.deltaTime);
        move = 0F;

        Halt = false;

    }

}
