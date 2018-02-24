using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using TC.Common;
using UMA;
using TC.UM4GPConquest.Utility;
using System;
using System.Linq;

public class UMASelectionController : MonoBehaviour {

    public AssetLoaderController AssetLoaderController;
    private GameObject[] createdUmas;
    public GameObject[] getAllSpawnedAvators
    {
        get { return createdUmas; }
    }

    private void Awake()
    {
        createdUmas = new GameObject[CommonNames.avators.Length];
    }

    // Use this for initialization
    void Start () {
        AssetLoaderController.CacheAllUMA(CommonNames.assetsNameStreamingFolder);
        //Dictionary<string, UMATextRecipe> umasMap = AssetLoaderController.umaCharactersTemplates;
        createdUmas = UMAGenericHelper.SpawnAllAvator(createdUmas,
            AssetLoaderController,
            CommonNames.avators,
            transform,
            Vector3.zero,
            Quaternion.Euler(0,180,0));

        UMAGenericHelper.ToggleUmasActivation(createdUmas);
    }

    
}
