using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.Common {

    public static class CommonNames
    {
        
        public static readonly string umaCharacterAssetPath = "Assets/UM4GPConquest/SavedRecipes/";
        public static readonly string umaSavedRecipeFolderPath = "Assets/UM4GPConquest/SavedRecipes";
        public static readonly string umaCharacterAssetBundlePath = "Assets/StreamingAssets/";
        public static readonly string umaFolderGeneratorPrefabPath = "Assets/UM4GPConquest/Prefab/UMAGenerator.prefab";
        public static readonly string standardAssetThirdPersonControllerPath =
            "Assets/Standard"+" "+"Assets/Characters/ThirdPersonCharacter/Animator/";
        /*this is the name used by uma characters stored in the StreamingAssets folder
         * **/
        public static readonly string assetsNameStreamingFolder = "umacharactersasset";
        //uma avators name
        public static readonly string[] avators = new string[] { "HumanMale", "HumanMale 1", "HumanMale 2", "HumanMale 3", "HumanMale 4"
            , "HumanFemale" , "HumanFemale 1", "HumanFemale 2", "HumanFemale 3", "HumanFemale 4"};

        #region Tags
        public static readonly string SERVER_OBJ_CONTROLLER_TAG = "ServerController";
        public static readonly string CLIENT_OBJ_CONTROLLER_TAG = "GameStatusController";
        public static readonly string TILE_TAG = "Tile";
        public static readonly string TOWER_TAG= "Tower";
        public static readonly string AVATOR_TAG = "Avator";
        public static readonly string DESTINATION_TAG = "Destination";
        #endregion
    }

}


