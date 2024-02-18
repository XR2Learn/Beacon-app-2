using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.PackageManager;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum Tags {
    NonInteractive,
    Erlenmeyer500ml,
    HeatKnob,
    StirKnob,
    ACSwitch,
    CeramicPlate,
    LightIntensityKnob,
    CondenserKnob,
    RevolvingNosepiece,
    Plug,
    CoarseFocusKnob,
    FineFocusKnob,
    OcularKnob,
    OcularLens,
    SpecimenHolder,
    StageKnob,
    SpecimenHolderKnob,
    Slide,
    ApertureKnob,
    CupboardDoor,
    Drawer,
    Cap
}


public enum Languages {
    EN = 0,
    GR = 1
}


public class OrdinaryNames {

    public List<string> LanguageSpecificNames = new List<string>();

    public OrdinaryNames(string _NameEn, string _NameGr) {
        LanguageSpecificNames.Add(_NameEn);
        LanguageSpecificNames.Add(_NameGr);
    }

}



public class MouseUI: MonoBehaviour {

    static Texture2D FingerCursor, HandCursor, GrabCursor, ClickingFingerCursor, EyeCursor;

    static Vector2 mouse_offset_for_finger = new Vector2(9, 2);
    static Vector2 mouse_offset_for_hand = new Vector2(11, 3);
    static Vector2 mouse_offset_for_grab = new Vector2(7, 2);
    static Vector2 mouse_offset_for_spin_arrow = new Vector2(15, 10);
    static Vector2 mouse_offset_for_eye = new Vector2(12, 12);

    //cursor identifiers
    public const int hand_cursor = 0;
    public const int grab_cursor = 1;
    public const int finger_cursor = 2;
    public const int clicking_finger_cursor = 3;
    public const int spin_arrow_cursor = 4;
    public const int eye_cursor = 5;

    public static Event occurence;

    static int Tooltip_MiddleX;
    static int Tooltip_MiddleY;

    static int Tooltip_SizeX;
    static int Tooltip_SizeY;

    static Vector2 Coords_for_FrozenToolip;

    static string Text_for_Tooltip;

    static Texture2D TooltipBG;

    public Tags Tag;

    [HideInInspector]
    public bool Pressable, Rotatable, Movable, Zoomable;

    public bool Impenetrable;

    string UserFriendlyName;

    static Camera mainCamera;

    static float Camera_Z_Distance;

    static GameObject ObjectHoveringOn;
    static GameObject ObjectBeingDragged;

    static List<GameObject> BlockingObjects;

    Vector3 VectorDistanceFromCamera;

    float MinimalDistanceForObjectToBeVisible;

    static GameObject GameObjectMouseIsDown_On;

    static GameObject gameObject2;

    static bool Rotating;

    static bool MovableObjectIsMoving;

    static bool MovableObjectHasJustBegunMovingVertically;
    static bool VerticalMoveDetected;

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(MouseUI)), CanEditMultipleObjects]
    public class MouseUI_Editor: Editor {

        public override void OnInspectorGUI() {

            base.OnInspectorGUI();

            MouseUI mouseUI = (MouseUI)target;

            if (mouseUI.Tag != Tags.NonInteractive) {
                showAttributes(mouseUI);
                assignBooleanValues(mouseUI);
            }
        }

        void showAttributes(MouseUI _mouseUI) {

            EditorGUILayout.BeginVertical();

            _mouseUI.Pressable = EditorGUILayout.Toggle("Pressable", _mouseUI.Pressable);
            _mouseUI.Rotatable = EditorGUILayout.Toggle("Rotatable", _mouseUI.Rotatable);
            _mouseUI.Movable = EditorGUILayout.Toggle("Movable", _mouseUI.Movable);
            _mouseUI.Zoomable = EditorGUILayout.Toggle("Zoomable", _mouseUI.Zoomable);

            EditorGUILayout.EndVertical();

        }

        void assignBooleanValues(MouseUI _mouseUI) {
            _mouseUI.Pressable = BooleanValues[_mouseUI.Tag][0];
            _mouseUI.Rotatable = BooleanValues[_mouseUI.Tag][1];
            _mouseUI.Movable = BooleanValues[_mouseUI.Tag][2];
            _mouseUI.Zoomable = BooleanValues[_mouseUI.Tag][3];
        }

    }

#endif    
    #endregion


    // Start is called before the first frame update
    void Start() {

        FingerCursor = Resources.Load("finger") as Texture2D;
        HandCursor = Resources.Load("hand") as Texture2D;
        GrabCursor = Resources.Load("grab") as Texture2D;
        ClickingFingerCursor = Resources.Load("clicking_finger") as Texture2D;
        EyeCursor = Resources.Load("eye") as Texture2D;

        Tooltip_MiddleX = 120;

        Tooltip_SizeX = 2 * Tooltip_MiddleX;

        Tooltip_SizeY = 50;
        Tooltip_MiddleY = Tooltip_SizeY + 5;

        TooltipBG = Resources.Load("ui_tooltip") as Texture2D;

        mainCamera = Camera.main;

        if (Tag == Tags.NonInteractive)
            MinimalDistanceForObjectToBeVisible = 1000F; //in order to take into account any bench
        else if (Tag == Tags.CupboardDoor || Tag == Tags.Drawer)
            MinimalDistanceForObjectToBeVisible = 3F;
        else
            MinimalDistanceForObjectToBeVisible = 1.5F;

        ObjectHoveringOn = null;
        ObjectBeingDragged = null;

        BlockingObjects = new List<GameObject>();

        GameObjectMouseIsDown_On = null;

        gameObject2 = null;

        Rotating = false;

        MovableObjectIsMoving = false;

        MovableObjectHasJustBegunMovingVertically = false;
        VerticalMoveDetected = false;

        UserFriendlyName = AttributedName(Tag);

    }


    void OnMouseOver() {

        VectorDistanceFromCamera = transform.position - mainCamera.transform.position;

        if (!Rotating && VectorDistanceFromCamera.magnitude < MinimalDistanceForObjectToBeVisible) {

            ObjectHoveringOn = gameObject;

            if (Tag != Tags.NonInteractive) {

                Text_for_Tooltip = UserFriendlyName;

                if (GameObjectMouseIsDown_On == null && ObjectBeingDragged == null) {

                    if (Pressable)
                        setCursor(finger_cursor);
                    else if (Zoomable)
                        setCursor(eye_cursor);
                    else if (Rotatable || Movable)
                        setCursor(hand_cursor);

                }
                else if (GameObjectMouseIsDown_On != gameObject) {

                    gameObject2 = gameObject;

                }

            }

        }

    }


    void OnMouseExit() {

        if (Tag != Tags.NonInteractive) {

            if (ObjectBeingDragged == null) {

                ObjectHoveringOn = null;
                Text_for_Tooltip = null;

                resetCursor();

            }
            else
                gameObject2 = null;

        }

    }


    void OnMouseDown() {

        if (Tag != Tags.NonInteractive) {

            if (ObjectHoveringOn == gameObject) {

                GameObjectMouseIsDown_On = gameObject;

                if (Movable) {

                    Camera_Z_Distance = mainCamera.WorldToScreenPoint(transform.position).z;
                    //it needs to be calucated here, since the object will be being moved at this
                    //camera distance

                    setCursor(grab_cursor);

                    if (GetComponent<InteractiveObject>().Place == null) //not to straighten objects who are in/on other objects and have got new but legit rotation
                        GetComponent<MovableObject>().restoreUprightRotation();
                }
                else if (Rotatable) {

                    Coords_for_FrozenToolip =
                        new Vector2(occurence.mousePosition.x, occurence.mousePosition.y);

                    setCursor(grab_cursor);

                }
                else if (Pressable) {

                    GetComponent<InteractiveObject>().press();

                    setCursor(clicking_finger_cursor);

                }
                else if (Zoomable) {

                    GetComponent<InteractiveObject>().zoom();

                    resetCursor();

                }

            }

        }

    }


    void OnMouseDrag() {

        if (Tag != Tags.NonInteractive && GameObjectMouseIsDown_On == gameObject) { /* so that you won't able to drag an object if you are not close enough to it */

            if (Rotating || BlockingObjects.Count == 0) {/* this condition secures that objects cannot be dragged through another interactive object (unless a rotatable is being rotated) */

                if (Rotatable || Movable)
                    ObjectBeingDragged = gameObject;

                float mouse_dx = Input.GetAxis("Mouse X");
                float mouse_dy = Input.GetAxis("Mouse Y");

                if (Movable && (MovableObjectIsMoving
                    || (mouse_dx != 0F || mouse_dy != 0F))) {
                    //movement of mouse is checked because when the object is pressed down on, it gets a different position from the original one as mouse cursor's position, when converted to world screen co-ords, is different from object's co-ords
                    //MovableObjectIsMoving is checked in order to secure that the object will stay in the air as long as the mouse button is pressed, even if the mouse is not moving

                    MovableObjectIsMoving = true;

                    freezeObjectRotation();

                    Vector3 Screen_Position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera_Z_Distance); //added z axis to screen point

                    Vector3 New_World_Position = mainCamera.ScreenToWorldPoint(Screen_Position);

                    transform.position = New_World_Position;

                    if (!MovableObjectHasJustBegunMovingVertically &&
                        mouse_dy != 0F) {

                        MovableObjectHasJustBegunMovingVertically = true;

                        if (mouse_dy < 0F)
                            VerticalMoveDetected = true;

                    }

                    if (GetComponent<InteractiveObject>().Place != null)
                        GetComponent<InteractiveObject>().Place.GetComponent<InteractiveObject>().evacuate(gameObject);

                }
                else if (Rotatable && occurence.isMouse) {
                    //without "occurence.isMouse == true", object is not properly rotated

                    Rotating = true;

                    Vector2 dv = occurence.delta;

                    GetComponent<InteractiveObject>().pivot(dv);

                }

            }

        }

    }


    void OnMouseUp() {

        bool ObjectedAllowedToRoll;

        ObjectedAllowedToRoll = (Movable) ? true : false;

        if (ObjectBeingDragged == gameObject && gameObject2 != null && gameObject2 != gameObject
            && !Rotatable && !Pressable && GetComponent<InteractiveObject>()) {
            //gameObject2 !== gameObject needs to be checked because with OnMouseDrag (), gameObject2 may become equal to gameObject and ObjectBeingDragged

            bool JointUse_Result = GetComponent<MouseUI>().tryCombining_With(gameObject2);

            if (JointUse_Result)
                ObjectedAllowedToRoll = false;

        }
        else if (Rotating) {

            GetComponent<InteractiveObject>().done_pivoting();

            Rotating = false;

        }

        if (ObjectedAllowedToRoll)
            GetComponent<MouseUI>().letObjectRoll();

        MovableObjectHasJustBegunMovingVertically = false;
        VerticalMoveDetected = false;

        MovableObjectIsMoving = false;
        GameObjectMouseIsDown_On = null;

        ObjectHoveringOn = null; //it needs to be set to null every time, otherwise the tooltip may continue after done rotating a knob or moving an object

        ObjectBeingDragged = null;
        gameObject2 = null;

        BlockingObjects.Clear();

        resetCursor();

    }


    bool tryCombining_With(GameObject _GameObject2) {

        Debug.Log($"MouseUI.tryCombiningWith, this: {gameObject.name}, with: {_GameObject2.name}");

        Values_After_JointUse ResultValues =
            _GameObject2.GetComponent<InteractiveObject>().use_with(gameObject);

        if (ResultValues.JointUse_TookPlace) {

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            //gameObject2's handling needs to be BEFORE gameObject1's because gameObject1 might be child of gameObject2
            if (ResultValues.gameObject2_NewPlace != null)
                _GameObject2.GetComponent<InteractiveObject>().Place = ResultValues.gameObject2_NewPlace;

            _GameObject2.GetComponent<MouseUI>().setInteractivity(
                ResultValues.gameObject2_NewInteractivity);

            if (ResultValues.gameObject1_NewPlace != null)
                gameObject.GetComponent<InteractiveObject>().Place = ResultValues.gameObject1_NewPlace;

            GetComponent<MouseUI>().setInteractivity(
                ResultValues.gameObject1_NewInteractivity);

        }

        return ResultValues.JointUse_TookPlace;

    }


    void freezeObjectRotation() {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }


    void letObjectRoll() {
        GetComponent<Rigidbody>().freezeRotation = false;
    }


    private void OnCollisionStay(Collision _collision) {

        if (VerticalMoveDetected) {

            if (ObjectBeingDragged == gameObject) {

                if (_collision.gameObject.GetComponent<MouseUI>() && _collision.gameObject.GetComponent<MouseUI>().Impenetrable &&
                    !BlockingObjects.Contains(_collision.gameObject)) {

                    BlockingObjects.Add(_collision.gameObject);


                }

            }

            VerticalMoveDetected = false;

        }

    }


    void OnCollisionEnter(Collision _collision) {

        if (!Rotating) {

            if (ObjectBeingDragged == gameObject) {

                if (_collision.gameObject.GetComponent<MouseUI>() && _collision.gameObject.GetComponent<MouseUI>().Impenetrable &&
                    !BlockingObjects.Contains(_collision.gameObject)) {

                    BlockingObjects.Add(_collision.gameObject);

                }

            }

        }

    }


    void OnCollisionExit(Collision _collision) {

        if (!Rotating) {

            if (ObjectBeingDragged == gameObject)
                BlockingObjects.Remove(_collision.gameObject);


        }

    }


    public static void setCursor(int _cursor_type) {

        switch (_cursor_type) {
            case hand_cursor:
                Cursor.SetCursor(HandCursor, mouse_offset_for_hand, CursorMode.Auto);
                break;
            case grab_cursor:
                Cursor.SetCursor(GrabCursor, mouse_offset_for_grab, CursorMode.Auto);
                break;
            case finger_cursor:
                Cursor.SetCursor(FingerCursor, mouse_offset_for_finger, CursorMode.Auto);
                break;
            case clicking_finger_cursor:
                Cursor.SetCursor(ClickingFingerCursor, mouse_offset_for_finger, CursorMode.Auto);
                break;
            case spin_arrow_cursor:
                Cursor.SetCursor(GrabCursor, mouse_offset_for_spin_arrow, CursorMode.Auto);
                break;
            case eye_cursor:
                Cursor.SetCursor(EyeCursor, mouse_offset_for_eye, CursorMode.Auto);
                break;
        }

    }


    public static void resetCursor() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    public static void callOnGUI() {

        occurence = Event.current;

        if (!Rotating && ObjectHoveringOn != null && ObjectBeingDragged != ObjectHoveringOn &&
            ObjectHoveringOn.GetComponent<MouseUI>().Tag != Tags.NonInteractive)
            DisplayTooltip();
        else if (Rotating)
            DisplayFrozenTooltip();

    }


    static void DisplayTooltip() {
        GUI.skin.box.alignment = TextAnchor.MiddleCenter;
        GUI.skin.box.normal.background = TooltipBG;
        GUI.skin.box.fontSize = 16;
        GUI.skin.box.normal.textColor = Color.yellow;
        GUI.backgroundColor = new Color(0F, 0F, 0F, 0.3F);
        GUI.Box(new Rect(occurence.mousePosition.x - Tooltip_MiddleX,
            occurence.mousePosition.y - Tooltip_MiddleY, Tooltip_SizeX, Tooltip_SizeY),
            Text_for_Tooltip);
    }

    static void DisplayFrozenTooltip() {
        GUI.skin.box.alignment = TextAnchor.MiddleCenter;
        GUI.skin.box.normal.background = TooltipBG;
        GUI.skin.box.fontSize = 16;
        GUI.skin.box.normal.textColor = Color.yellow;
        GUI.backgroundColor = new Color(0F, 0F, 0F, 0.3F);
        GUI.Box(new Rect(Coords_for_FrozenToolip.x - Tooltip_MiddleX,
                Coords_for_FrozenToolip.y - Tooltip_MiddleY, Tooltip_SizeX, Tooltip_SizeY),
                Text_for_Tooltip);
    }


    public void setInteractivity(bool _InteractivityState) {

        for (int i = 0; i < GetComponents<BoxCollider>().Length; i++)
            GetComponents<BoxCollider>()[i].enabled = _InteractivityState;

        //if (!_object.GetComponent<pipette> ()) {

        for (int i = 0; i < transform.childCount; i++) {

            transform.GetChild(i).GetComponent<MouseUI>().setInteractivity(_InteractivityState);

        }

    }


    static readonly Dictionary<Tags, OrdinaryNames> NameAttribution = new Dictionary<Tags, OrdinaryNames> {
        {Tags.NonInteractive, new OrdinaryNames(null,null)},
        {Tags.Erlenmeyer500ml, new OrdinaryNames ("ERLENMEYER 500ml","йымийг жиакг 500ml")},
        {Tags.HeatKnob, new OrdinaryNames ("HEAT KNOB","йовкиас хеяламсгс")},
        {Tags.StirKnob, new OrdinaryNames ("STIR KNOB","йовкиас амадеусгс")},
        {Tags.ACSwitch, new OrdinaryNames ("SWITCH","диайоптгс")},
        {Tags.CeramicPlate, new OrdinaryNames ("CERAMIC PLATE","йеяалийг пкайа")},
        {Tags.LightIntensityKnob, new OrdinaryNames ("LIGHT INTENSITY KNOB","йовкиас емтасгс жытос")},
        {Tags.CondenserKnob, new OrdinaryNames ("CONDENSER KNOB","йовкиас сулпуймытг")},
        {Tags.RevolvingNosepiece, new OrdinaryNames ("REVOLVING NOSEPIECE","пеяистяежолемг йежакг")},
        {Tags.Plug, new OrdinaryNames ("PLUG","йакыдио")},
        {Tags.CoarseFocusKnob, new OrdinaryNames("COARSE FOCUS KNOB", "адяос йовкиас")},
        {Tags.FineFocusKnob, new OrdinaryNames("FINE FOCUS KNOB", "лийяолетяийос йовкиас")},
        {Tags.OcularKnob, new OrdinaryNames ("OCULAR LENS","пяосожхаклиос жайос")},
        {Tags.OcularLens, new OrdinaryNames ("OCULAR LENS","пяосожхаклиос жайос")},
        {Tags.SpecimenHolder, new OrdinaryNames ("SPECIMEN HOLDER", "одгцос деицлатос")},
        {Tags.StageKnob, new OrdinaryNames ("STAGE KNOB","йовкиас тяапефас")},
        {Tags.SpecimenHolderKnob, new OrdinaryNames ("SPECIMEN HOLDER KNOB","йовкиас одгцоу деицлатос")},
        {Tags.Slide, new OrdinaryNames ("SPECIMEN","деицла")},
        {Tags.ApertureKnob, new OrdinaryNames ("APERTURE KNOB","ловкос ияидас")},
        {Tags.CupboardDoor, new OrdinaryNames ("CUPBOARD DOOR","поята мтоукапиоу")},
        {Tags.Drawer, new OrdinaryNames ("DRAWER","суятаяи")},
        {Tags.Cap, new OrdinaryNames ("CAP", "йапайи")}
    };

    public static string AttributedName(Tags _NameTag) {
        return NameAttribution[_NameTag].LanguageSpecificNames[(int)Specs.Language];
    }


    public static readonly Dictionary<Tags, List<bool>> BooleanValues =
        new Dictionary<Tags, List<bool>>() {
        //1st: Pressable; 2nd: Rotatable; 3rd: Movable; 4th: Zoomable
        {Tags.Erlenmeyer500ml, new List<bool> {false,false,true,false}},
        {Tags.HeatKnob, new List<bool> {false,true,false,false}},
        {Tags.StirKnob, new List<bool> {false,true,false,false}},
        {Tags.ACSwitch, new List<bool> {true,false,false,false}},
        {Tags.CeramicPlate, new List<bool> {false,false,false,false}},
        {Tags.LightIntensityKnob, new List<bool> {false,true,false,false}},
        {Tags.CondenserKnob, new List<bool> {false,true,false,false}},
        {Tags.RevolvingNosepiece, new List<bool> {false,true,false,false}},
        {Tags.Plug, new List<bool> {false,false,true,false}},
        {Tags.CoarseFocusKnob, new List<bool> {false,true,false,false}},
        {Tags.FineFocusKnob, new List<bool> {false,true,false,false}},
        {Tags.OcularKnob, new List<bool> {false,true,false,false}},
        {Tags.OcularLens, new List<bool> {false,false,false,true}},
        {Tags.SpecimenHolder, new List<bool> {false,false,false,false}},
        {Tags.StageKnob, new List<bool> {false,true,false,false}},
        {Tags.SpecimenHolderKnob, new List<bool> {false,true,false,false}},
        {Tags.Slide, new List<bool> {false,false,true,false}},
        {Tags.ApertureKnob, new List<bool> {false,true,false,false}},
        {Tags.CupboardDoor, new List<bool> {false,true,false,false}},
        {Tags.Drawer, new List<bool> {false,true,false,false}},
        {Tags.Cap, new List<bool> {false,false,true,false}},
    };

}
