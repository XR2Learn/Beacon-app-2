using com.ganast.log.unity;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


//
// TODO: MERGE WITH, or CLEARLY SEPARATE FROM, VlabXRInteractiveObject which should in turn extend or cooperate
// with XRGrabInteractable or something (must combine functionality of grabbagles and not grabbables) !!! Good
// idea seems to be extensions of XRBaseInteractable for clicks/turns, XRGrabInteractable for moving around and
// combining, etc., each with its own event handling so that, as an added benefit, we won't have to manually set
// up event callbacks to other handler scripts on each and every interactable.
//

namespace com.ganast.xr2learn.vlab {

    /**
     * 
     */
    [RequireComponent(typeof(InteractiveObject))]
    public class InteractiveObjectXRInteractable: XRBaseInteractable {

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
        private string tooltip;

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

            MouseUI mouseUI = GetComponent<MouseUI>();
            if (mouseUI != null) {
                // TODO: this is not quite right, the tooltip to be displayed for each object along with other
                // information of this kind should be exposed by the object itself and, to add, not by a MouseUI
                // component whose name refers to a certain functionality but by a tooltip data or general object
                // data provider component with a proper API...
                tooltip = MouseUI.AttributedName(mouseUI.Tag);

                // TODO: create and initialize tooltip display billboard GUI structure...
            }
            else {
                tooltip = "#" + name;
            }
        }

        /**
         * 
         */
        protected override void OnHoverEntered(HoverEnterEventArgs args) {

            base.OnHoverEntered(args);

            Log.Message(this, "OnHoverEntered", "interactable: " + name + ", interactor: " + args.interactorObject.transform.name);

            // TODO: display tooltip in world...
            Log.Message(this, "OnHoverEntered", "tooltip: " + tooltip);
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
        }

        /**
         * 
         */
        protected void Update() {

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
    }
}
