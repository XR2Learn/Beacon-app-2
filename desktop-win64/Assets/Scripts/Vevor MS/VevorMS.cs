using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VevorMS : Instrument {

    #region Editor
#if UNITY_EDITOR

    [CustomEditor (typeof (VevorMS)), CanEditMultipleObjects]
    public class VevorMS_Editor : Instrument_Editor {
        public override void OnInspectorGUI () {

            base.OnInspectorGUI ();

            VevorMS vevorMS= (VevorMS)target;

            EditorGUILayout.BeginVertical ();

            vevorMS.ControlMonitor = EditorGUILayout.ObjectField ("Control Monitor", vevorMS.ControlMonitor, typeof (GameObject), true) as GameObject;

            EditorGUILayout.EndVertical ();

            showPlace ();

        }

    }

#endif    
    #endregion

    public override void Start () {

        base.Start ();

    }

}
