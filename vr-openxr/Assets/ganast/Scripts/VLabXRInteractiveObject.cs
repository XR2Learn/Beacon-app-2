using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/**
 * VLab XR Interactive Object
 * 
 * @author ganast
 */

public class VLabXRInteractiveObject: MonoBehaviour {

    [SerializeField]
    private VLabInteractionManager vLabInteractionManager;

    private bool isGrabbed = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (isGrabbed) {
            InteractiveObject o = vLabInteractionManager.GetNearestInteractiveObjectInRange(transform, false);
            if (o != null) {
                Debug.Log("!!!" + o.name);
            }
        }
    }

    public void OnSelectEntered(SelectEnterEventArgs e) {
        if (e.interactableObject is XRGrabInteractable) {
            Debug.Log("Grabbed");
            isGrabbed = true;
        }
    }

    public void OnSelectExited(SelectExitEventArgs e) {

        if (e.interactableObject is XRGrabInteractable) {

            Debug.Log("Ungrabbed");
            isGrabbed = false;

            InteractiveObject o = vLabInteractionManager.GetNearestInteractiveObjectInRange(transform, false);

            Values_After_JointUse ResultValues = o.use_with(gameObject);

            if (ResultValues.JointUse_TookPlace) {

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                if (ResultValues.gameObject2_NewPlace != null) {
                    o.Place = ResultValues.gameObject2_NewPlace;
                }

                // o.GetComponent<MouseUI>().setInteractivity(
                //     ResultValues.gameObject2_NewInteractivity);

                if (ResultValues.gameObject1_NewPlace != null) {
                    GetComponent<InteractiveObject>().Place = ResultValues.gameObject1_NewPlace;
                }

                // GetComponent<MouseUI>().setInteractivity(
                //     ResultValues.gameObject1_NewInteractivity);
            }
        }
    }
}
