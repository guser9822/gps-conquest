using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using UMA;
using System;
using System.Linq;

namespace TC.UM4GPConquest.Utility
{
    public static class UMAGenericHelper
    {
        /**
         * Given an AssetLoaderController in input,a UMA avator's name and a UMADynamicAvator
         * create and spawn a new uma avator. It is possible to pass also a callback
         * that it's executed after the uma avator is created.
         * */
        public static GameObject createUMAAvator(AssetLoaderController assetLoaderController,
            string umaAvatorName,
            UMADynamicAvatar umaDynamicAvator,
            Action<UMAData> onCharacterCreatedCallback)
        {
            GameObject thisUma = null;
            if (assetLoaderController != null && umaAvatorName != null
                && umaDynamicAvator!=null)
            {
                //Set/spawn a UMA Avator
                thisUma = umaDynamicAvator.gameObject;
                UMADynamicAvatar thisUmaDynamicAvator = thisUma.GetComponent<UMADynamicAvatar>();

                thisUmaDynamicAvator.context = assetLoaderController.context;
                thisUmaDynamicAvator.umaGenerator = assetLoaderController.generator;
                thisUmaDynamicAvator.loadOnStart = false;
                thisUmaDynamicAvator.Initialize();
                thisUmaDynamicAvator.animationController = assetLoaderController.thirdPersonController;
                UMATextRecipe recipe = assetLoaderController.umaCharactersTemplates[umaAvatorName];
                thisUmaDynamicAvator.Load(recipe);

                if(onCharacterCreatedCallback!=null)
                    thisUma.GetComponent<UMAData>().OnCharacterCreated += onCharacterCreatedCallback;
            }
            else
            {
                string partOfTheError = "given in input is null";
                if (assetLoaderController==null)
                    throw new System.ArgumentException("AssetLoader "+partOfTheError);
                else if (umaAvatorName == null)
                    throw new System.ArgumentException("UMA's avator name " + partOfTheError);
                else if(umaDynamicAvator==null)
                    throw new System.ArgumentException("DyanamicAvator " + partOfTheError);

            }
            return thisUma;
        }

        /*
     * Given a non empty array of GameObject and an AssetLoaderController, spawn all the avators available
     * and uses the array to get a reference to them. It is possibile to set rotation,position and a parent.
     * Umas are generated according the array of names given in input.
     * */
        public static GameObject[] SpawnAllAvator(GameObject[] umasSet,
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
                umasSet[x].transform.rotation = umasRotation;
            }
            return umasSet;
        }

        /* 
         * Deactivates all the uma's except the one that correspond to the index
         * given in input. If the index parameter doesn't get valorized, it means 
         * that all the uma's will be deactivated.
         * */
        public static GameObject ToggleUmasActivation(GameObject[] umasSet, int _toActivate = int.MaxValue)
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

        private static void _ToggleUmasActivationInternalLogic(GameObject[] umasSet, Action<GameObject> _action)
        {
            Array.ForEach(umasSet, _action);
        }

        private static bool IsIndexInUmas(GameObject[] umas, int index)
        {
            return Enumerable.Range(0, umas.Length).Contains<int>(index);
        }

        private static bool IsWantedUma(GameObject _wantedUma, GameObject _maybe)
        {
            return _maybe.Equals(_wantedUma);
        }

    }
}

