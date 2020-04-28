using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomReset : MonoBehaviour {

    public GameObject GrabbablesRoot;
    public GameObject GrabbablesPrefab;

    public void ResetGrabbables() {
        Destroy(GrabbablesRoot);
        GrabbablesRoot = Instantiate(GrabbablesPrefab);
    }

}
