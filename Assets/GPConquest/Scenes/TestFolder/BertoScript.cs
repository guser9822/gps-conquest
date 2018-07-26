using System.Collections;
using System.Collections.Generic;
using TC.GPConquest.MarkLight4GPConquest.Player;
using TC.GPConquest.Player;
using UnityEngine;

public class BertoScript : MonoBehaviour {

    public Camera camerA;
    public GameObject AvatorUIPrefab;
    public GameObject InstantiatedAvatorUI;
    public string myName;
    public AvatorUI avatorUI;

    // Use this for initialization
    void Start () {
        InstantiatedAvatorUI = Instantiate(AvatorUIPrefab);
        InstantiatedAvatorUI.transform.SetParent(this.transform);
        InstantiatedAvatorUI.transform.localPosition = new Vector3(0, 2, 0);
        PlayerEntity p = new PlayerEntity();
        p.username = myName;
        avatorUI = InstantiatedAvatorUI.GetComponentInChildren<AvatorUI>();
        avatorUI.InitAvatorUI(camerA, p);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
