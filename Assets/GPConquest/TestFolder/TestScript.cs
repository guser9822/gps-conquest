using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

    public tileGen tileGen;

	// Use this for initialization
	void Start () {
        StartCoroutine(tileGen.StartTiling());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
