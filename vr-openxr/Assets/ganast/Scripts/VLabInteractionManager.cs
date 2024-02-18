using System.Collections.Generic;

using UnityEngine;

/**
 * VLab Interaction Manager
 * 
 * @author ganast
 */
public class VLabInteractionManager: MonoBehaviour {

    public static float INTERACTIVITY_RANGE_MIN = 1.5f;

    /**
     * The list of interactive objects in the scene on application run start.
     */
    private InteractiveObject[] interactiveObjects = null;

    /**
     * The list of objects that are being dragged at any given moment. This can include any type of object, not
     * just interactive ones.
     */
    private List<Transform> draggedObjects = new List<Transform>();

    /**
     * 
     */
    public void Awake() {

        // locate all interactive objects, include inactive ones since, in principle, an interactive object may
        // start as inactive and get activated later depending on the scenario, also no need to sort as the list
        // is meant to be iterated over in its entirety to identify overlaps, etc., and not for accessing
        // individual entries...
        interactiveObjects = FindObjectsByType<InteractiveObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        Debug.Log($"[VLabInteractionManager] Found {interactiveObjects.Length} interactive object(s):");
        foreach (InteractiveObject o in interactiveObjects) {
            Debug.Log($"[VLabInteractionManager] {o.name}");
            // TODO: validate each interactive object and provide feedback in the log and/or disable objects if
            // illegally configured...
        }
    }

    public void Start() {
    }

    /**
     * 
     */
    public InteractiveObject GetNearestInteractiveObjectInRange(Transform target, bool includeInactive) {

        InteractiveObject r = null;
        float l = INTERACTIVITY_RANGE_MIN;

        foreach (InteractiveObject o in interactiveObjects) {
            if ((!includeInactive || o.isActiveAndEnabled) && !o.transform.Equals(target)) {
                float d = Vector3.Distance(o.transform.position, target.transform.position);
                if (d < l) {
                    r = o;
                    l = d;
                }
            }
        }
        return r;
    }
}
