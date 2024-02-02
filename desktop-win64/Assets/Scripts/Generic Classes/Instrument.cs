using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Instrument : MonoBehaviour {

    public const bool ON = true;
    public const bool OFF = false;

    public const bool connected = true;
    public const bool disconnected = false;

    public const bool locked = true;
    public const bool unlocked = false;

    [HideInInspector]
    public GameObject ControlLamp, ControlMonitor, ControlPanel, ControlExtraUI;

    [HideInInspector]
    public bool View;

    public static GameObject Ego;
    public static EgoController Ego_Controller;

    [HideInInspector]
    public Modes Focus_Mode;

    [HideInInspector]
    public float Focus_PosX_Offset, Focus_PosZ_Offset, Focus_Camera_RotX, Focus_Field_of_View, Focus_Theta;

    float Y_Angle;
    float dx, dz;

    [HideInInspector]
    public bool State;

    bool PowerController_State;
    [HideInInspector]
    public bool Cable_State; //needs to be public so as to be checked externally

    [HideInInspector]
    public GameObject Place;

    #region Editor
#if UNITY_EDITOR

    [CustomEditor (typeof (Instrument)), CanEditMultipleObjects]
    public class Instrument_Editor : Editor {
        
        public override void OnInspectorGUI () {
            
            base.OnInspectorGUI ();

            EditorGUILayout.Space ();

        }

        public void showPlace () {

            Instrument instrument = (Instrument)target;

            EditorGUILayout.Space ();

            instrument.Place = EditorGUILayout.ObjectField ("Place", instrument.Place, typeof (GameObject), true) as GameObject;

        }

    }

#endif    
    #endregion


    // Start is called before the first frame update
    public virtual void Start () {

        Ego = GameObject.Find ("Ego");

        if (ControlPanel != null)
            setFocusValues ();

        Cable_State = connected; //Temporary, until we implement the socket
        PowerController_State = OFF;
        
        State = OFF;

    }

    void setFocusValues () {

        View = unlocked;

        Ego_Controller = Ego.GetComponent<EgoController> ();

        float Orientation = Orientation_in_3D_Space ();

        Y_Angle = Orientation + Focus_Theta;

        if (Orientation == 0F) {
            dx = Focus_PosX_Offset;
            dz = Focus_PosZ_Offset;
        } else if (Orientation == 90F) {
            dx = Focus_PosZ_Offset;
            dz = -Focus_PosX_Offset;
        } else if (Orientation == 180F) {
            dx = -Focus_PosX_Offset;
            dz = -Focus_PosZ_Offset;
        } else {
            dx = -Focus_PosZ_Offset;
            dz = Focus_PosX_Offset;
        }

    }

    float Orientation_in_3D_Space () {
        return GetComponent<Instrument> ().Place.transform.localEulerAngles.y;
    }


    public void toggleView () {

        if (View == unlocked)
            lockView ();
        else
            unlockView ();

        View = !View;

        if (ControlExtraUI != null)
            ControlExtraUI.GetComponent<ExtraUI> ().setActivationStatus (View);

    }


    public virtual void lockView () {
        Ego_Controller.setMode (Focus_Mode, gameObject, transform.localPosition.x + dx,
            transform.localPosition.z + dz, Y_Angle, Focus_Camera_RotX, Focus_Field_of_View);
    }


    public virtual void unlockView () {
        Ego_Controller.setMode (Modes.Navigation);
    }

    public virtual void updateFeedback_from_Cable (bool _New_Cable_State) {
        
        Cable_State = _New_Cable_State;        
        
        updateState ();

        if (ControlPanel != null)
            ControlPanel.GetComponent<Panel> ().updateZoomability (_New_Cable_State);

    }


    public void updateFeedback_from_PowerController (bool _New_PowerController_State) {
        
        PowerController_State = _New_PowerController_State;
        
        updateState ();

    }


    void updateState () {

        State = Cable_State && PowerController_State;

        if (ControlMonitor != null)
            ControlMonitor.GetComponent<Monitor> ().updateState (State);

        if (ControlLamp != null)
            ControlLamp.GetComponent<Lamp> ().updateState (State);

    }


    //public Vector3 Offset;
    //public float XAngle;
    //public float FoV;


    //public void Update () {
    //    Offset = Ego.transform.localPosition - transform.localPosition;
    //	XAngle = Ego.transform.localEulerAngles.x;
    //	FoV = Ego.GetComponent<Camera> ().fieldOfView;
    //}


}
