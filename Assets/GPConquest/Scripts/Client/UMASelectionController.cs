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
        createdUmas = SpawnAllAvator(createdUmas,
            AssetLoaderController,
            CommonNames.avators,
            transform,
            Vector3.zero,
            Quaternion.Euler(0,180,0));

        ToggleUmasActivation(createdUmas,6);
    }

    /*
     * Given a non empty array of GameObject and an AssetLoaderController, spawn all the avators available
     * and uses the array to get a reference to them. It is possibile to set rotation,position and a parent.
     * Umas are generated according the array of names given in input.
     * */
    private GameObject[] SpawnAllAvator(GameObject[] umasSet,
        AssetLoaderController _assetLoaderController,
        string[] arrayOfNames,
        Transform _parent,
        Vector3 umasPosition,
        Quaternion umasRotation
        )
    {
        for (int x = 0; x < umasSet.Length; x++)
        {
            umasSet[x] = new GameObject();
            umasSet[x].gameObject.AddComponent<UMADynamicAvatar>();
            umasSet[x].gameObject.name = arrayOfNames[x];
            umasSet[x] = UMAGenericHelper.createUMAAvator(_assetLoaderController,
                arrayOfNames[x],
                umasSet[x].gameObject.GetComponent<UMADynamicAvatar>(),
                null);
            umasSet[x].transform.parent = _parent;
            umasSet[x].transform.position = umasPosition;
            umasSet[x].transform.rotation= umasRotation;
        }
        return umasSet;
    }

    /* 
     * Deactivates all the uma's except the one that correspond to the index
     * given in input. If the index parameter doesn't get valorized, it means 
     * that all the uma's will be deactivated.
     * */
    public GameObject ToggleUmasActivation(GameObject[] umasSet, int _toActivate = int.MaxValue)
    {
        GameObject result = null;
        if (IsIndexInUmas(umasSet, _toActivate))
        {
            result = umasSet[_toActivate];
            _ToggleUmasActivationInternalLogic(umasSet, x =>
                {
                    if (IsWantedUma(x, umasSet[_toActivate]))
                        umasSet[_toActivate].gameObject.SetActive(true);
                    else x.SetActive(false);
                });
        }
        else _ToggleUmasActivationInternalLogic(umasSet, x => x.SetActive(false));
        return result;
    }

    private void _ToggleUmasActivationInternalLogic(GameObject[] umasSet,Action<GameObject> _action)
    {
        Array.ForEach(umasSet,_action);
    }

    private bool IsIndexInUmas(GameObject[] umas, int index)
    {
        return Enumerable.Range(0, umas.Length).Contains<int>(index);
    }

    private bool IsWantedUma(GameObject _wantedUma,GameObject _maybe)
    {
        return _maybe.Equals(_wantedUma);
    }
}
