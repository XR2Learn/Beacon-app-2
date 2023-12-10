using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PhotonicMicroscope : Instrument {

    #region Editor
#if UNITY_EDITOR

    [CustomEditor (typeof (PhotonicMicroscope)), CanEditMultipleObjects]
    public class PhotonicMicroscope_Editor : Instrument_Editor {
        
        public override void OnInspectorGUI () {

            base.OnInspectorGUI ();

            PhotonicMicroscope photonicMiscroscope = (PhotonicMicroscope)target;

            EditorGUILayout.BeginVertical ();

            photonicMiscroscope.ControlLamp = EditorGUILayout.ObjectField ("Control Lamp", photonicMiscroscope.ControlLamp, typeof (GameObject), true) as GameObject;
            photonicMiscroscope.ControlPanel = EditorGUILayout.ObjectField ("Control Ocular Lens", photonicMiscroscope.ControlPanel, typeof (GameObject), true) as GameObject;
            photonicMiscroscope.ControlExtraUI = EditorGUILayout.ObjectField ("Control Microscoping UI", photonicMiscroscope.ControlExtraUI, typeof (GameObject), true) as GameObject;

            EditorGUILayout.EndVertical ();

            base.showPlace ();

        }

    }

#endif    
    #endregion

    public GameObject ControlSpecimenHolder;

    public override void Start () {

        Focus_Mode = Modes.Microscoping;
        
        Focus_PosX_Offset = 0.5819168F;
        Focus_PosZ_Offset = -0.704891F;
        
        Focus_Camera_RotX = 16F;
        
        Focus_Field_of_View = 44F;
        Focus_Theta = -13.61F;
        
        base.Start ();

    }


    public override void lockView () {

        base.lockView ();

        ControlSpecimenHolder.GetComponent<PhotonicMicroscope_SpecimenHolder> ().setActivation_of_SpecimenMovement (false);

    }

    public override void unlockView () {
        
        base.unlockView ();

        ControlSpecimenHolder.GetComponent<PhotonicMicroscope_SpecimenHolder> ().setActivation_of_SpecimenMovement (true);

    }


}
