using UnityEngine;

public class Values_After_JointUse {

	public bool JointUse_TookPlace;

	public GameObject gameObject1_NewPlace;
	public GameObject gameObject2_NewPlace;

	public bool gameObject1_NewInteractivity;
	public bool gameObject2_NewInteractivity;


	public Values_After_JointUse (bool _JointUse_TookPlace, GameObject _NewPlace1,
		bool _NewInteractivity1, GameObject _NewPlace2, bool _NewInteractivity2) {
	
		JointUse_TookPlace = _JointUse_TookPlace;

		gameObject1_NewPlace = _NewPlace1;
		gameObject1_NewInteractivity = _NewInteractivity1;

		gameObject2_NewPlace = _NewPlace2;
		gameObject2_NewInteractivity = _NewInteractivity2;

	}


	public Values_After_JointUse (bool _JointUse_TookPlace) {
		//called only with false argument
		JointUse_TookPlace = _JointUse_TookPlace;
	}

}
