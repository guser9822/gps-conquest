using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight;

public class TempCubeController : MonoBehaviour {

    public GameObject AvatorUI;
    protected GameObject AvatorUIViewPresenter;
    public Camera Camera;

	// Use this for initialization
	void Start () {

        AvatorUIViewPresenter = Instantiate<GameObject>(AvatorUI,
            new Vector3(transform.position.x,transform.position.y+3,transform.position.z),
            Quaternion.identity);

        AvatorUIViewPresenter.transform.parent = transform;

        AvatorUIViewPresenter.transform.rotation = 
            Quaternion.RotateTowards(AvatorUIViewPresenter.transform.rotation, Camera.transform.rotation,360);


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
