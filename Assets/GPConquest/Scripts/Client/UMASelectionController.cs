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

    public enum VERSE{
        NEXT,
        PREV
    }

    public AssetLoaderController AssetLoaderController;
    private GameObject[] createdUmas;
    public GameObject[] getAllSpawnedAvators
    {
        get { return createdUmas; }
    }
    private int currentIndexOfTheSelection = 0;

    private void Awake()
    {
        createdUmas = new GameObject[CommonNames.avators.Length];
    }

    // Use this for initialization
    void Start () {
        //Load the asset containing all the uma's generated in the editor 
        AssetLoaderController.CacheAllUMA(CommonNames.assetsNameStreamingFolder);

        //Create and spawn all the uma's
        createdUmas = UMAGenericHelper.SpawnAllAvator(createdUmas,
            AssetLoaderController,
            CommonNames.avators,
            transform,
            Vector3.zero,
            Quaternion.Euler(0,180,0),
            x => 
            {
                //Deactivate shadows production and reception
                x.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().shadowCastingMode =
                    UnityEngine.Rendering.ShadowCastingMode.Off;
                x.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().receiveShadows = false;
            });

        //Deactivate all the uma's execept the first
        UMAGenericHelper.ToggleUmasActivation(createdUmas,0);
    }

    public bool ChangeUMA(VERSE verse)
    {
        currentIndexOfTheSelection = ApplyVerse(verse, currentIndexOfTheSelection);
        return UMAGenericHelper.ToggleUmasActivation(createdUmas, currentIndexOfTheSelection);
    }

    /*
     * Cycle the index circularly between 0 and the number of the uma's
     * **/
    private int ApplyVerse(VERSE verse,int _index)
    {
        if (verse == VERSE.NEXT)
            _index++;
        else _index--;
        return StayInRange(_index);
    }

    /*
    * Don't let the index stay outside the uma's array lenght
    * **/
    private int StayInRange(int _index)
    {
        return _index < 0 ? createdUmas.Length - 1 : _index > createdUmas.Length - 1 ? 0 : _index;
    }

    
}
