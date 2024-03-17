using System.Collections.Generic;

using com.ganast.log.unity;

using TMPro;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace com.ganast.xr2learn.vlab {

    /**
     * 
     */
    [RequireComponent(typeof(InteractiveObject))]
    public class InteractiveObjectXRInteractable: XRBaseInteractable {

        [SerializeField]
        private Tags Tag;

        /**
         * 
         */
        [SerializeField]
        private Vector3 translationWeights;

        /**
         * 
         */
        [SerializeField]
        private Vector3 rotationWeights;

        /**
         * 
         */
        [SerializeField]
        private float clickThreshold;

        /**
         * 
         */
        [SerializeField]
        private float translationThreshold;

        /**
         * 
         */
        [SerializeField]
        private float rotationThreshold;

        /**
         * 
         */
        [SerializeField]
        private bool transposeRotation;


        /**
         * A target of type InteractiveObject, required for the operation of this script.
         */
        private InteractiveObject target;

        /**
         * 
         */
        private string tooltipText;

        /**
         * 
         */
        IXRInteractor interactor = null;

        /**
         * 
         */
        private Vector3 oldInteractorPosition;

        /**
         * 
         */
        private Quaternion oldInteractorRotation;

        /**
         * 
         */
        private float dt = 0.0f;

        /**
         * 
         */
        protected override void Awake() {

            base.Awake();

            target = GetComponent<InteractiveObject>();

            tooltipText = AttributedName(Tag);
        }

        /**
         * 
         */
        protected override void OnHoverEntered(HoverEnterEventArgs args) {

            base.OnHoverEntered(args);

            Log.Message(this, "OnHoverEntered", "interactable: " + name + ", interactor: " + args.interactorObject.transform.name);

            Log.Message(this, "OnHoverEntered", "tooltip: " + tooltipText);

            TooltipManager.GetTooltipManager().SetTooltipText(tooltipText);
            TooltipManager.GetTooltipManager().SetTooltipVisible(true);
        }

        protected override void OnHoverExited(HoverExitEventArgs args) {
            base.OnHoverExited(args);
            TooltipManager.GetTooltipManager().SetTooltipVisible(false);
        }

        /**
         * 
         */
        protected override void OnActivated(ActivateEventArgs args) {

            base.OnActivated(args);

            // interactor = args.interactorObject;

            // Log.Message(this, "OnActivated", "interactable: " + name + ", interactor: " + interactor.transform.name);

            // oldInteractorPosition = interactor.transform.position;
            // oldInteractorRotation = interactor.transform.rotation;
        }

        /**
         * 
         */
        protected override void OnDeactivated(DeactivateEventArgs args) {

            base.OnDeactivated(args);

            // Log.Message(this, "OnDeactivated", "interactable: " + name + ", interactor: " + interactor.transform.name);

            // interactor = null;
        }

        /**
         * 
         */
        protected override void OnSelectEntered(SelectEnterEventArgs args) {

            base.OnSelectEntered(args);

            TooltipManager.GetTooltipManager().SetTooltipVisible(false);
            TooltipManager.GetTooltipManager().Freeze();

            interactor = args.interactorObject;

            interactor.transform.GetComponent<XRInteractorLineVisual>().enabled = false;

            Log.Message(this, "OnSelectEntered", "interactable: " + name + ", interactor: " + args.interactorObject.transform.name);

            dt = 0.0f;

            oldInteractorPosition = interactor.transform.position;
            oldInteractorRotation = interactor.transform.rotation;
        }

        /**
         * 
         */
        protected override void OnSelectExited(SelectExitEventArgs args) {

            base.OnSelectExited(args);

            if (dt < clickThreshold) {
                target.press();
                target.zoom();
                Log.Message(this, "OnSelectExited", "pressed, target: " + name);
            }
            else {
                target.done_pivoting();
            }

            dt = 0.0f;

            Log.Message(this, "OnSelectExited", "interactable: " + name + ", interactor: " + args.interactorObject.transform.name);

            interactor.transform.GetComponent<XRInteractorLineVisual>().enabled = true;

            interactor = null;

            TooltipManager.GetTooltipManager().Unfreeze();
            TooltipManager.GetTooltipManager().SetTooltipVisible(false);
        }

        /**
         * 
         */
        protected void Update() {

            TooltipManager.GetTooltipManager().LookAt(Camera.main.transform);

            if (interactor != null) {

                dt += Time.deltaTime;

                if (dt > clickThreshold) {

                    Vector3 newInteractorPosition = interactor.transform.position;
                    Quaternion newInteractorRotation = interactor.transform.rotation;

                    Vector3 positionDelta = (newInteractorPosition - oldInteractorPosition);
                    positionDelta.Scale(translationWeights);
                    Vector3 eulerDelta = (newInteractorRotation.eulerAngles - oldInteractorRotation.eulerAngles);
                    eulerDelta.Scale(rotationWeights);
                    float angleDelta = Quaternion.Angle(newInteractorRotation, oldInteractorRotation);

                    // Log.Message(this, "Update", "turning, target: " + name + ", position delta: " + positionDelta
                    //     + ", rotation delta: " + eulerDelta + " (" + angleDelta + ")");

                    if (positionDelta.sqrMagnitude > translationThreshold || angleDelta > rotationThreshold) {
                        Vector2 rotation = transposeRotation ?
                            new Vector2(positionDelta.y + eulerDelta.x, positionDelta.x + eulerDelta.y) :
                        new Vector2(positionDelta.x + eulerDelta.y, positionDelta.y + eulerDelta.x);
                        target.pivot(rotation);
                        oldInteractorPosition = newInteractorPosition;
                        oldInteractorRotation = newInteractorRotation;
                    }
                }
            }
        }

        /* --- Tooltip-specific, original implementation by vzaf ----------------------------------------------- */

        public static string AttributedName(Tags _NameTag) {
            return NameAttribution[_NameTag].LanguageSpecificNames[(int) Specs.Language];
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
            {Tags.OcularKnob, new OrdinaryNames ("OCULAR KNOB","пяосожхаклиос йовкиас")},
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
    }
}
