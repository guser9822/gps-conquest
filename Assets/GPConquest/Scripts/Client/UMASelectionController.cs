using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using TC.Common;
using UMA;
using TC.UM4GPConquest.Utility;
using System;

public class UMASelectionController : MonoBehaviour {

    public AssetLoaderController AssetLoaderController;
    public GameObject[] createdUmas;

    private void Awake()
    {
        createdUmas = new GameObject[CommonNames.avators.Length];
        for (int x=0; x < CommonNames.avators.Length; x++) {
            createdUmas[x] = new GameObject();
            createdUmas[x].gameObject.AddComponent<UMADynamicAvatar>();
        }
        //Array.ForEach(createdUmas, x => x = new GameObject().AddComponent<UMADynamicAvatar>().gameObject);
    }

    // Use this for initialization
    void Start () {
        AssetLoaderController.CacheAllUMA(CommonNames.assetsNameStreamingFolder);
        Dictionary<string,UMATextRecipe> umasMap = AssetLoaderController.umaCharactersTemplates;

        int i = CommonNames.avators.Length;

        createdUmas = Array.ConvertAll<string, GameObject>(CommonNames.avators,
            x => UMAGenericHelper.
            createUMAAvator(AssetLoaderController,
            x,
            createdUmas[i=(i-1)].gameObject.GetComponent<UMADynamicAvatar>(),
            null));

        //AssetLoaderController.umaCharactersAsset.Unload(true);
        //AssetBundle.UnloadAllAssetBundles(true);
    }


	// Update is called once per frame
	void Update () {
		
	}
}
