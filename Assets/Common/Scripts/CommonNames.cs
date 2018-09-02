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
        public static readonly string[] avators = new string[] 
        {   "HumanMale",
            "HumanMale 1",
            "HumanMale 2",
            "HumanMale 3",
            "HumanMale 4",
            "HumanMale 5",
            "HumanMale 6",
            "HumanMale 7",
            "HumanMale 8",
            "HumanMale 9",
            "HumanMale 10",
            "HumanMale 11",
            "HumanMale 12",
            "HumanMale 13",
            "HumanMale 15",
            "HumanMale 15",
            "HumanMale 16",
            "HumanMale 17",
            "HumanMale 18",
            "HumanMale 19",
            "HumanMale 20",
            "HumanMale 21",
            "HumanMale 22",
            "HumanMale 23",
            "HumanFemale",
            "HumanFemale 1",
            "HumanFemale 2",
            "HumanFemale 3",
            "HumanFemale 4",
            "HumanFemale 5",
            "HumanFemale 6",
            "HumanFemale 7",
            "HumanFemale 8",
            "HumanFemale 9",
            "HumanFemale 10",
            "HumanFemale 11",
            "HumanFemale 12",
            "HumanFemale 13",
            "HumanFemale 14",
            "HumanFemale 15",
            "HumanFemale 16",
            "HumanFemale 17",
            "HumanFemale 18",
            "HumanFemale 19",
            "HumanFemale 21",
            "HumanFemale 22",
            "HumanFemale 23",
            "HumanFemale 24",
            "HumanFemale 25",
            "HumanFemale 26",
            "HumanFemale 27"
        };

        #region Tags
        public static readonly string SERVER_OBJ_CONTROLLER_TAG = "ServerController";
        public static readonly string CLIENT_OBJ_CONTROLLER_TAG = "GameStatusController";
        public static readonly string TILE_TAG = "Tile";
        public static readonly string TOWER_TAG= "Tower";
        public static readonly string AVATOR_TAG = "Avator";
        public static readonly string DESTINATION_TAG = "Destination"; 
        public static readonly string MAIN_CAMERA_TAG = "MainCamera";
        public static readonly string GAME_FACTIONS_CONTROLLER = "GameFactionsController";

        #endregion
    }

}


