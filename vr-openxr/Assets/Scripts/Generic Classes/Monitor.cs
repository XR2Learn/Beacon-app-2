using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour {

    public GameObject Indications;

    // Start is called before the first frame update
    public virtual void Start () {
        Indications.SetActive (false);
    }

    public void updateState (bool _InstrumentState) {
        Indications.SetActive (_InstrumentState);
    }

}
