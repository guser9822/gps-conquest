#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UMA;
using UnityEngine.AI;
using System.IO;
using TC.GPConquest.Player;
using TC.Common;

namespace TC.UM4GPConquest
{
    #region Uma initialization / Characters creation,save,load
    public static class UMAInitializer
    {

        [MenuItem("UM4GPConquest/Build UMA asset/Windows x64")]
        static void BuildUMAssetW64()//Build asset for loading character at runtime
        {
            string[] GPConquestAssets = AssetDatabase.FindAssets(null, new string[] { CommonNames.umaSavedRecipeFolderPath });

            if (GPConquestAssets != null && GPConquestAssets.Length > 0)
            {
                List<string> umaCharactersAssetNamesPath = new List<string>();
                foreach (string asset in GPConquestAssets)
                {
                    umaCharactersAssetNamesPath.Add(AssetDatabase.GUIDToAssetPath(asset));
                }
                umaCharactersAssetNamesPath.
                    Add(CommonNames.standardAssetThirdPersonControllerPath + "ThirdPersonAnimatorController.controller");

                AssetBundleBuild[] assetBundleBuild = new AssetBundleBuild[1];
                assetBundleBuild[0].assetBundleName = "UMACharactersAsset";
                //assetBundleBuild[0].addressableNames = umaCharactersAssetNames;
                assetBundleBuild[0].assetNames = umaCharactersAssetNamesPath.ToArray();

                BuildPipeline.BuildAssetBundles(CommonNames.umaCharacterAssetBundlePath,
                    assetBundleBuild,
                    BuildAssetBundleOptions.None,
                    BuildTarget.StandaloneWindows64);

            }

            //string[] umaCharactersAssetNames = new string[3];
            //umaCharactersAssetNames[0] = "HumanMale";
            //umaCharactersAssetNames[1] = "HumanFemale";
            //umaCharactersAssetNames[2] = "ThirdPersonController";

            //string[] umaCharactersAssetNamesPath = new string[3];
            //umaCharactersAssetNamesPath[0] = CommonNames.umaCharacterAssetPath + "HumanMale.asset";
            //umaCharactersAssetNamesPath[1] = CommonNames.umaCharacterAssetPath + "HumanFemale.asset";
            //umaCharactersAssetNamesPath[2] = CommonNames.standardAssetThirdPersonControllerPath + "ThirdPersonAnimatorController.controller";

            //AssetBundleBuild[] assetBundleBuild = new AssetBundleBuild[1];
            //assetBundleBuild[0].assetBundleName = "UMACharactersAsset";
            //assetBundleBuild[0].addressableNames = umaCharactersAssetNames;
            //assetBundleBuild[0].assetNames = umaCharactersAssetNamesPath;

            //BuildPipeline.BuildAssetBundles(CommonNames.umaCharacterAssetBundlePath,
            //    assetBundleBuild,
            //    BuildAssetBundleOptions.None,
            //    BuildTarget.StandaloneWindows64);

        }

        [MenuItem("UM4GPConquest/Build UMA asset/Android")]
        static void BuildUMAssetAndroid()//Build asset for loading character at runtime
        {
            string[] umaCharactersAssetNames = new string[3];
            umaCharactersAssetNames[0] = "HumanMale";
            umaCharactersAssetNames[1] = "HumanFemale";
            umaCharactersAssetNames[2] = "ThirdPersonController";

            string[] umaCharactersAssetNamesPath = new string[3];
            umaCharactersAssetNamesPath[0] = CommonNames.umaCharacterAssetPath + "HumanMale.asset";
            umaCharactersAssetNamesPath[1] = CommonNames.umaCharacterAssetPath + "HumanFemale.asset";
            umaCharactersAssetNamesPath[2] = CommonNames.standardAssetThirdPersonControllerPath + "ThirdPersonAnimatorController.controller";

            AssetBundleBuild[] assetBundleBuild = new AssetBundleBuild[1];
            assetBundleBuild[0].assetBundleName = "UMACharactersAssetAndroid";
            assetBundleBuild[0].addressableNames = umaCharactersAssetNames;
            assetBundleBuild[0].assetNames = umaCharactersAssetNamesPath;

            BuildPipeline.BuildAssetBundles(CommonNames.umaCharacterAssetBundlePath,
                assetBundleBuild,
                BuildAssetBundleOptions.None,
                BuildTarget.Android);
        }

        [MenuItem("UM4GPConquest/Init UMA")]
        static void InitializeUMA()
        {//Initialize UMA context
         // UMA
            GameObject uma = new GameObject();
            uma.name = "UMA";
            GameObject parent;
            if (parent = Selection.activeGameObject)
            {
                uma.transform.SetParent(parent.transform);
            }

            // Race Library
            GameObject races = new GameObject();
            races.name = "RaceLibrary";
            races.transform.SetParent(uma.transform);
            RaceLibrary raceLibrary = races.AddComponent<RaceLibrary>();

            //null search in all assets folder, t means type, so we are searching Uma.RaceData
            string[] racesData = AssetDatabase.FindAssets("t: RaceData", null);
            foreach (string data in racesData)
            {
                //get the path
                string racePath = AssetDatabase.GUIDToAssetPath(data);
                RaceData raceAsset = AssetDatabase.LoadAssetAtPath<RaceData>(racePath);
                raceLibrary.AddRace(raceAsset);
            }

            // Slots
            GameObject slots = new GameObject();
            slots.name = "SlotLibrary";
            slots.transform.SetParent(uma.transform);

            SlotLibrary slotLibrary = slots.AddComponent<SlotLibrary>();

            //null search in all assets folder, t means type, so we are searching Uma.RaceData
            string[] slotsData = AssetDatabase.FindAssets("t: SlotDataAsset", null);
            foreach (string data in slotsData)
            {
                //get the path
                string slotPath = AssetDatabase.GUIDToAssetPath(data);
                SlotDataAsset slotAsset = AssetDatabase.LoadAssetAtPath<SlotDataAsset>(slotPath);
                slotLibrary.AddSlotAsset(slotAsset);
            }

            // Overlays
            GameObject overlays = new GameObject();
            overlays.name = "OverlayLibrary";
            overlays.transform.SetParent(uma.transform);

            OverlayLibrary overlayLibrary = overlays.AddComponent<OverlayLibrary>();

            //null search in all assets folder, t means type, so we are searching Uma.RaceData
            string[] overlaysData = AssetDatabase.FindAssets("t: OverlayDataAsset", null);
            foreach (string data in overlaysData)
            {
                //get the path
                string overlayPath = AssetDatabase.GUIDToAssetPath(data);
                OverlayDataAsset overlayAsset = AssetDatabase.LoadAssetAtPath<OverlayDataAsset>(overlayPath);
                overlayLibrary.AddOverlayAsset(overlayAsset);
            }

            // Context
            GameObject context = new GameObject();
            context.name = "ContextLibrary";
            context.transform.SetParent(uma.transform);

            UMAContext umaContext = context.AddComponent<UMAContext>();
            umaContext.raceLibrary = raceLibrary;
            umaContext.slotLibrary = slotLibrary;
            umaContext.overlayLibrary = overlayLibrary;

            // Generators
            UMAGenerator generator = MonoBehaviour.Instantiate<UMAGenerator>
                (AssetDatabase.LoadAssetAtPath<UMAGenerator>(CommonNames.umaFolderGeneratorPrefabPath));
            generator.name = "UMAGenerator";
            generator.transform.SetParent(uma.transform);

            UMADefaultMeshCombiner meshCombiner = generator.gameObject.AddComponent<UMADefaultMeshCombiner>();
            generator.meshCombiner = meshCombiner;

            //Mixers for random UMA creation
            //Male
            GameObject maleRecipeMixer = new GameObject();
            maleRecipeMixer.name = "MaleRecipeMixer";
            UMA.Examples.UMARecipeMixer maleRecipeMixerScript = maleRecipeMixer.AddComponent<UMA.Examples.UMARecipeMixer>();
            maleRecipeMixerScript.raceData = raceLibrary.GetRace("HumanMale");

            UMA.Examples.UMARecipeMixer.RecipeSection[] maleRecipeSection = new UMA.Examples.UMARecipeMixer.RecipeSection[3];

            //Male Recipe Section : Body
            maleRecipeSection[0] = new UMA.Examples.UMARecipeMixer.RecipeSection();
            maleRecipeSection[0].name = "Body";
            maleRecipeSection[0].selectionRule = UMA.Examples.UMARecipeMixer.SelectionType.IncludeOne;
            maleRecipeSection[0].recipes = new UMARecipeBase[2];
            string[] maleBodyRecipePath = AssetDatabase.FindAssets("MaleBase t: UMATextRecipe", null);
            maleRecipeSection[0].recipes[0] = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(maleBodyRecipePath[0]));
            string[] maleBody2RecipePath = AssetDatabase.FindAssets("MaleBase2 t: UMATextRecipe", null);
            maleRecipeSection[0].recipes[1] = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(maleBody2RecipePath[0]));

            //Male Recipe Section : Underwear
            maleRecipeSection[1] = new UMA.Examples.UMARecipeMixer.RecipeSection();
            maleRecipeSection[1].name = "Clothing";
            maleRecipeSection[1].selectionRule = UMA.Examples.UMARecipeMixer.SelectionType.IncludeOne;
            maleRecipeSection[1].recipes = new UMARecipeBase[1];
            //string[] maleUnderwearRecipePath = AssetDatabase.FindAssets("MaleUnderwear t: UMATextRecipe", null);
            //maleRecipeSection[1].recipes[0] = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(maleUnderwearRecipePath[0]));
            string[] maleOutfit1RecipePath = AssetDatabase.FindAssets("MaleOutfit1 t: UMATextRecipe", null);
            foreach (string hairPath in maleOutfit1RecipePath)
            {
                UMATextRecipe umaTextRecipe = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(hairPath));
                if (umaTextRecipe.name == "MaleOutfit1")
                    maleRecipeSection[1].recipes[0] = umaTextRecipe;
            }

            //Male Recipe Section : Hair
            maleRecipeSection[2] = new UMA.Examples.UMARecipeMixer.RecipeSection();
            maleRecipeSection[2].name = "Hair";
            maleRecipeSection[2].selectionRule = UMA.Examples.UMARecipeMixer.SelectionType.IncludeSome;
            maleRecipeSection[2].recipes = new UMARecipeBase[2];
            string[] maleBeardRecipePath = AssetDatabase.FindAssets("MaleBeard t: UMATextRecipe", null);
            foreach (string hairPath in maleBeardRecipePath)
            {
                UMATextRecipe umaTextRecipe = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(hairPath));
                if (umaTextRecipe.name == "MaleBeard")
                    maleRecipeSection[2].recipes[0] = umaTextRecipe;
            }
            string[] maleHairRecipePath = AssetDatabase.FindAssets("MaleHair t: UMATextRecipe", null);
            foreach (string hairPath in maleHairRecipePath)
            {
                UMATextRecipe umaTextRecipe = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(hairPath));
                if (umaTextRecipe.name == "MaleHair")
                    maleRecipeSection[2].recipes[1] = umaTextRecipe;
            }

            maleRecipeMixerScript.recipeSections = maleRecipeSection;

            maleRecipeMixer.GetComponent<Transform>().SetParent(uma.transform);

            //Female
            GameObject femaleRecipeMixer = new GameObject();
            femaleRecipeMixer.name = "FemaleRecipeMixer";
            UMA.Examples.UMARecipeMixer femaleRecipeMixerScript = femaleRecipeMixer.AddComponent<UMA.Examples.UMARecipeMixer>();
            femaleRecipeMixerScript.raceData = raceLibrary.GetRace("HumanFemale");
            UMA.Examples.UMARecipeMixer.RecipeSection[] femaleRecipeSection = new UMA.Examples.UMARecipeMixer.RecipeSection[3];

            //Female Recipe Section : Body
            femaleRecipeSection[0] = new UMA.Examples.UMARecipeMixer.RecipeSection();
            femaleRecipeSection[0].name = "Body";
            femaleRecipeSection[0].selectionRule = UMA.Examples.UMARecipeMixer.SelectionType.IncludeOne;
            femaleRecipeSection[0].recipes = new UMARecipeBase[1];
            string[] femaleBodyRecipePath = AssetDatabase.FindAssets("HumanFemale Base Recipe t: UMATextRecipe", null);
            femaleRecipeSection[0].recipes[0] = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(femaleBodyRecipePath[0]));

            //Female Recipe Section : Underwear
            femaleRecipeSection[1] = new UMA.Examples.UMARecipeMixer.RecipeSection();
            femaleRecipeSection[1].name = "Clothing";
            femaleRecipeSection[1].selectionRule = UMA.Examples.UMARecipeMixer.SelectionType.IncludeOne;
            femaleRecipeSection[1].recipes = new UMARecipeBase[2];
            //string[] femaleUnderwearRecipePath = AssetDatabase.FindAssets("FemaleUnderwear t: UMATextRecipe", null);
            string[] femaleOutfit1RecipePath = AssetDatabase.FindAssets("FemaleOutfit1 t: UMATextRecipe", null);
            string[] femaleOutfit2RecipePath = AssetDatabase.FindAssets("FemaleOutfit2 t: UMATextRecipe", null);
            //femaleRecipeSection[1].recipes[0] = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(femaleUnderwearRecipePath[0]));
            femaleRecipeSection[1].recipes[0] = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(femaleOutfit1RecipePath[0]));
            femaleRecipeSection[1].recipes[1] = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(femaleOutfit2RecipePath[0]));

            //Female Recipe Section : Hair
            femaleRecipeSection[2] = new UMA.Examples.UMARecipeMixer.RecipeSection();
            femaleRecipeSection[2].name = "Hair";
            femaleRecipeSection[2].selectionRule = UMA.Examples.UMARecipeMixer.SelectionType.IncludeOne;
            femaleRecipeSection[2].recipes = new UMARecipeBase[3];
            string[] femaleHairRecipePath = AssetDatabase.FindAssets("FemaleHair t: UMATextRecipe", null);
            string[] femaleHairLongRecipePath = AssetDatabase.FindAssets("FemaleHairLong t: UMATextRecipe", null);
            string[] femaleHairShortRecipePath = AssetDatabase.FindAssets("FemaleHairShort t: UMATextRecipe", null);

            foreach (string hairPath in femaleHairRecipePath)
            {
                UMATextRecipe umaTextRecipe = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(hairPath));
                if (umaTextRecipe.name == "FemaleHair")
                    femaleRecipeSection[2].recipes[0] = umaTextRecipe;
            }

            femaleRecipeSection[2].recipes[1] = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(femaleHairLongRecipePath[0]));
            femaleRecipeSection[2].recipes[2] = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(AssetDatabase.GUIDToAssetPath(femaleHairShortRecipePath[0]));

            femaleRecipeMixerScript.recipeSections = femaleRecipeSection;

            femaleRecipeMixer.GetComponent<Transform>().SetParent(uma.transform);

            //Shared colors table
            GameObject recipeMixerController = new GameObject();
            recipeMixerController.name = "RecipeMixerController";
            RecipeMixerController recipeMixerControllerScript = recipeMixerController.AddComponent<RecipeMixerController>();
            SharedColorTable[] sharedColorTable = recipeMixerControllerScript.sharedColors;

            string[][] sharedColorsPath = new string[5][];
            sharedColorsPath[0] = AssetDatabase.FindAssets("HumanHairLinear", null);
            sharedColorsPath[1] = AssetDatabase.FindAssets("HumanSkinLinear", null);
            sharedColorsPath[2] = AssetDatabase.FindAssets("ClothingUnderwear", null);
            sharedColorsPath[3] = AssetDatabase.FindAssets("ClothingTops", null);
            sharedColorsPath[4] = AssetDatabase.FindAssets("ClothingBottoms", null);

            sharedColorTable[0] = AssetDatabase.LoadAssetAtPath<SharedColorTable>(AssetDatabase.GUIDToAssetPath(sharedColorsPath[0][0]));
            sharedColorTable[1] = AssetDatabase.LoadAssetAtPath<SharedColorTable>(AssetDatabase.GUIDToAssetPath(sharedColorsPath[1][0]));
            sharedColorTable[2] = AssetDatabase.LoadAssetAtPath<SharedColorTable>(AssetDatabase.GUIDToAssetPath(sharedColorsPath[2][0]));
            sharedColorTable[3] = AssetDatabase.LoadAssetAtPath<SharedColorTable>(AssetDatabase.GUIDToAssetPath(sharedColorsPath[3][0]));
            sharedColorTable[4] = AssetDatabase.LoadAssetAtPath<SharedColorTable>(AssetDatabase.GUIDToAssetPath(sharedColorsPath[4][0]));

            recipeMixerController.transform.SetParent(uma.transform);


        }

        //Create a UMA chracter with random recipe mixer
        [MenuItem("UM4GPConquest/Create a UMA")]
        static void CreatePrimitiveUma()//Spawn a UMA charater and save its recipe in 'SavedRecipes' folder
        {
            UMAGenerator generator = MonoBehaviour.FindObjectOfType<UMAGenerator>();

            GameObject uma = new GameObject();
            uma.name = "A primitive UMA";
            UMADynamicAvatar dynamicAvator = uma.AddComponent<UMADynamicAvatar>();
            dynamicAvator.loadOnStart = true;

            //animator
            string[] animator = AssetDatabase.FindAssets("ThirdPerson t: RuntimeAnimatorController"); // Standard asset animator
            string animatorPath = AssetDatabase.GUIDToAssetPath(animator[0]);
            dynamicAvator.animationController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(animatorPath);

            //npc movement script
            UMAController umaController = uma.AddComponent<UMAController>();
            umaController.speed = 0.4f;

            //character controller
            CharacterController characterController = uma.AddComponent<CharacterController>();
            characterController.center = new Vector3(0, 1.0f, 0);
            //Recipe mixer
            UMA.Examples.UMARecipeMixer[] recipesMixer = MonoBehaviour.FindObjectsOfType<UMA.Examples.UMARecipeMixer>();

            dynamicAvator.context = MonoBehaviour.FindObjectOfType<UMAContext>();
            dynamicAvator.umaGenerator = generator;
            dynamicAvator.Initialize();
            UMAData umaData = dynamicAvator.umaData;
            umaData.atlasResolutionScale = 1.0f;

            RecipeMixerController recipeMixerController = MonoBehaviour.FindObjectOfType<RecipeMixerController>();

            RandomizeRecipe(umaData
            , recipesMixer
            , dynamicAvator.context
            , recipeMixerController.sharedColors);

            RandomizeDNA(umaData);

            SaveRecipe(umaData, dynamicAvator.context);

            dynamicAvator.Show();
        }

        static void RandomizeRecipe(UMAData umaData, UMA.Examples.UMARecipeMixer[] recipeMixers, UMAContext context, SharedColorTable[] sharedColors)
        {
            UMA.Examples.UMARecipeMixer mixer = recipeMixers[Random.Range(0, recipeMixers.Length)];

            mixer.FillUMARecipe(umaData.umaRecipe, context);

            OverlayColorData[] recipeColors = umaData.umaRecipe.sharedColors;
            if ((recipeColors != null) && (recipeColors.Length > 0))
            {
                foreach (var sharedColor in sharedColors)
                {
                    if (sharedColor == null) continue;
                    int index = Random.Range(0, sharedColor.colors.Length);
                    for (int i = 0; i < recipeColors.Length; i++)
                    {
                        if (recipeColors[i].name == sharedColor.sharedColorName)
                        {
                            recipeColors[i].color = sharedColor.colors[index].color;
                        }
                    }
                }
            }

            //This is a HACK - maybe there should be a clean way
            // of removing a conflicting slot via the recipe?
            int maleJeansIndex = -1;
            int maleLegsIndex = -1;
            SlotData[] slots = umaData.umaRecipe.GetAllSlots();
            for (int i = 0; i < slots.Length; i++)
            {
                SlotData slot = slots[i];
                if (slot == null) continue;
                if (slot.asset.name == null) continue;

                if (slot.asset.slotName == "MaleJeans01") maleJeansIndex = i;
                else if (slot.asset.slotName == "MaleLegs") maleLegsIndex = i;
            }
            if ((maleJeansIndex >= 0) && (maleLegsIndex >= 0))
            {
                umaData.umaRecipe.SetSlot(maleLegsIndex, null);
            }
        }

        static void RandomizeDNA(UMAData umaData)
        {
            RaceData race = umaData.umaRecipe.GetRace();
            if ((race != null) && (race.dnaRanges != null))
            {
                foreach (DNARangeAsset dnaRange in race.dnaRanges)
                {
                    dnaRange.RandomizeDNA(umaData);
                }
            }
        }

        static void SaveRecipe(UMAData umaData, UMAContext context)
        {
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(CommonNames.umaCharacterAssetPath, umaData.umaRecipe.raceData.raceName + ".asset"));
            var asset = ScriptableObject.CreateInstance<UMATextRecipe>();
            asset.Save(umaData.umaRecipe, context);
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
        }

        #endregion

    }

}
#endif


