using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using TC.Common;
using UMA;
using TC.UM4GPConquest.Utility;

public class UMASelectionController : MonoBehaviour {

    public AssetLoaderController AssetLoaderController;
    private UMADynamicAvatar UMADynamicAvator;

    private void Awake()
    {
        UMADynamicAvator = GetComponent<UMADynamicAvatar>();
    }

    // Use this for initialization
    void Start () {
        AssetLoaderController.CacheAllUMA(CommonNames.assetsNameStreamingFolder);
        Dictionary<string,UMATextRecipe> umasMap = AssetLoaderController.umaCharactersTemplates;

        GameObject g = UMAGenericHelper.createUMAAvator(AssetLoaderController,
            "HumanFemale 3", UMADynamicAvator,null);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
