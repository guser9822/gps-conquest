﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UnityEngine.AI;
using System.IO;
using TC.Common;

namespace TC.GPConquest.Player {

    public class AssetLoaderController : MonoBehaviour
    {

        public Dictionary<string, UMATextRecipe> umaCharactersTemplates = new Dictionary<string, UMATextRecipe>();
        public RuntimeAnimatorController thirdPersonController { get; protected set; }
        public AssetBundle umaCharactersAsset { get; protected set; }
        public UMAGenerator generator { get; protected set; }
        public UMAContext context { get; protected set; }

        public void CacheAllUMA(string assetName)
        {
            umaCharactersAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/"+assetName);
            Object[] allAssets = null;
            /*
             * The first process that execute AssetBundle.LoadFromFile will load the asset,
             * so we nned to check this before calling LoadAllAssets() or an exception
             * will be thrown.
             * **/
            if (umaCharactersAsset != null)
                allAssets = umaCharactersAsset.LoadAllAssets();
            else Debug.Log("Uma assets already loaded");

            //Cache all available UMAs
            foreach (Object asset in allAssets)
            {
                if (asset.GetType() == typeof(UMATextRecipe))
                {
                    UMATextRecipe umaRecipe = (UMATextRecipe)asset;
                    umaCharactersTemplates.Add(umaRecipe.name, umaRecipe);
                }
                else { thirdPersonController = (RuntimeAnimatorController)asset; }
            }

            generator = MonoBehaviour.FindObjectOfType<UMAGenerator>();
            context = MonoBehaviour.FindObjectOfType<UMAContext>();
        }
    }

}

