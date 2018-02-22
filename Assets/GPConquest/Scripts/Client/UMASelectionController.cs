using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using TC.Common;

public class UMASelectionController : MonoBehaviour {

    public AssetLoaderController AssetLoaderController;

	// Use this for initialization
	void Start () {
        AssetLoaderController.CacheAllUMA(CommonNames.assetsNameStreamingFolder);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
