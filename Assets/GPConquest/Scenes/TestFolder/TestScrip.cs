using MarkLight;
using System.Collections;
using System.Collections.Generic;
using TC.GPConquest.MarkLight4GPConquest.Server;
using UnityEngine;

public class TestScrip : MonoBehaviour {

    public ViewPresenter TowerUI;
    public ViewPresenter TowerUIInstantiated;
    public TowerEntityGameUI TowerEntityMainUI;

    // Use this for initialization
    void Start () {
        TowerUIInstantiated = Instantiate<ViewPresenter>(TowerUI);
        TowerEntityMainUI = TowerUIInstantiated.GetComponentInChildren<TowerEntityGameUI>();
        Debug.Log("Stop");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
