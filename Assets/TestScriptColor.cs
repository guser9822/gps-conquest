using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptColor : MonoBehaviour {

    public GameObject TowerEffect;
    public GameObject InstantiatedTowerEffect;

	// Use this for initialization
	void Start ()
    {
        InstantiatedTowerEffect = GameObject.Instantiate(TowerEffect);
        InstantiatedTowerEffect.transform.SetParent(this.transform);
        var mainRingParticleSystem = InstantiatedTowerEffect.GetComponent<ParticleSystem>();
        var mmAin = mainRingParticleSystem.main;
        mmAin.startColor = Color.red;

        var particleSystemInChildren = InstantiatedTowerEffect.GetComponentsInChildren<ParticleSystem>();
        foreach (var particleSystem in particleSystemInChildren)
        {
            var mainComp = particleSystem.main;
            mainComp.startColor = Color.red;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
