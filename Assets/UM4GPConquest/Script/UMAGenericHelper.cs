using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC.GPConquest.Player;
using UMA;
using System;

namespace TC.UM4GPConquest.Utility
{
    public static class UMAGenericHelper
    {
        /**
         * Given an AssetLoaderController in input,a UMA avator's name and a UMADynamicAvator
         * create and spawn a new uma avator. It is possible to pass also a callback
         * that it's executed when the uma avator is created.
         * */
        public static GameObject createUMAAvator(AssetLoaderController assetLoaderController,
            string umaAvatorName,
            UMADynamicAvatar umaDynamicAvator,
            Action<UMAData> onCharacterCreatedCallback)
        {
            if (assetLoaderController != null && umaAvatorName != null
                && umaDynamicAvator!=null)
            {
                //Set/spawn a UMA Avator
                GameObject thisUma = umaDynamicAvator.gameObject;
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
            return new GameObject();
        }
    }
}

