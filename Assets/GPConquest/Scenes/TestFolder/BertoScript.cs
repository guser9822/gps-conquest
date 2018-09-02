using System.Collections;
using System.Collections.Generic;
using TC.GPConquest.MarkLight4GPConquest.Player;
using TC.GPConquest.Player;
using UnityEngine;

public class BertoScript : MonoBehaviour {

    public GameObject CompassPrefab;
    [HideInInspector]
    public GameObject InstantiatedCompassPrefab;
    public GameObject CubeToFollow;
    protected Transform DaggerTransf;

    // Use this for initialization
    void Start () {
        InstantiatedCompassPrefab = Instantiate(CompassPrefab,transform);
        DaggerTransf = InstantiatedCompassPrefab.transform;
        //DaggerTransf.localPosition = new Vector3(0.0f, 2f,0.0f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(CubeToFollow.transform);
        DaggerTransf.LookAt(CubeToFollow.transform);
    }
}
