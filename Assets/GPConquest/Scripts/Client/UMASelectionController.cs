﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using TC.Common;
using UMA;

public class UMASelectionController : MonoBehaviour {

    public AssetLoaderController AssetLoaderController;

	// Use this for initialization
	void Start () {
        AssetLoaderController.CacheAllUMA(CommonNames.assetsNameStreamingFolder);
        Dictionary<string,UMATextRecipe> umasMap = AssetLoaderController.umaCharactersTemplates;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
