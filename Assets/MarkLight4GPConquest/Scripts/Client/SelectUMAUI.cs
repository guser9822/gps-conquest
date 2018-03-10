﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using TC.GPConquest.Player;
using MarkLight;
using TC.GPConquest.Common;
using TC.Common;
using UnityEngine.SceneManagement;
using System;
using TC.GPConquest.MarkLight4GPConquest.Common;

namespace TC.GPConquest.MarkLight4GPConquest
{
    public class SelectUMAUI : UIView
    {
        /*
         * NOTE : MarkLightUI have a mechanism of auto-reloading
         * of the views. Given and object X, automatically created
         * with MarkLightUI (through ViewPresenter and a new view),
         * if you try to put a script Y on X with the Inspector, then
         * when the view gets automatically refreshed, Y vanish in the
         * oblivion, and then, you lose any reference to it.
         * 
         * I think that, so, it's better to let MarkLightUI handles all the 
         * scripts for you, just declaring them as class's properties. This 
         * will also make more simple every dipendecy problem and will also 
         * avoid use of the methods like GameObject.FindObjectOfType<>().
         * */
        public UMASelectionController UmaSelectionController;
        public AssetLoaderController AssetLoaderController;
        public ObservableList<string> FactionsList;
        public ComboBox ComboBoxFactions;
        [MapTo("FactionsList.SelectedItem")]
        public _object XFaction;
        public InputField UsernameInput;
        public InputField PasswordInput;
        public InputField EmailInput;
        public Button ConfirmButton;
        protected string selectedUma = "HumanMale";
        public GenericPopUp GenericPopUp;

        /*
         * In order to see a default faction in the selection scene
         * we create an ObservableList overriding Initialize method of 
         * the MarkLightUI framework.
         * **/
        public override void Initialize()
        {
            base.Initialize();
            FactionsList = InitializeFactionsList(GameCommonNames.FACTIONS);
        }

        protected ObservableList<string> InitializeFactionsList(List<string> _factions)
        {
            var factionList = new ObservableList<string>();
            _factions.ForEach(x => factionList.Add(x));
            return factionList;
        }

        /*
         * After the ObservableList is created we choose here at which index
         * should reference, or rather, which faction should be picked and
         * showed in the selection scene. This operation doesn't work in the
         * Initialize method (the overrided one) called above.
         * **/
        private void Start()
        {
            FactionsList.SelectedIndex = 1;
            XFaction = ComboBoxFactions.SelectedItem;
        }

        public void NextUMA()
        {
           selectedUma = UmaSelectionController.ChangeUMA(UMASelectionController.VERSE.NEXT);
        }

        public void PrevUMA()
        {
            selectedUma = UmaSelectionController.ChangeUMA(UMASelectionController.VERSE.PREV);
        }

        public void CallBack()
        {
            //TODO : I think that it's not correct to load/unload all the assets everytime. This is a temporary solution
            AssetBundle.UnloadAllAssetBundles(true);
            SceneManager.LoadScene(GPCSceneManager.GetSceneIndex(GPCSceneManager.GPCSceneEnum.CLIENT_MENU));
        }

        public void CallConfirm()
        {
            //Find the object UserInformations in the scene and fill it with the informations
            UserInformations userInformations = FindObjectOfType<UserInformations>();
            userInformations.SetUserInformations(
             UsernameInput.Text.Value,
             PasswordInput.Text.Value,
             EmailInput.Text.Value,
             (string)XFaction.Value,
             selectedUma);
            GenericPopUp.ShowPopUp(UIInfoLayer.AccountCreatedMessage);
        }

        private void Update()
        {
            ChecksOnUI();
        }

        public void ChecksOnUI()
        {
            UIHelperUtility.ExecuteActionOnButton<String>(ConfirmButton,
                new List<String>() {UsernameInput.Text.Value,
                                        PasswordInput.Text.Value,
                                        EmailInput.Text.Value },
                x => x.TrueForAll(y => CheckIfToEnableButton(y)),
               (x,y) => y.IsVisible.Value = x);
        }

        private bool CheckIfToEnableButton(String _string)
        {
            return UIHelperUtility.ValidateString(_string) &&
                UIHelperUtility.ValidateString(_string) &&
                UIHelperUtility.ValidateString(_string);
        }

    }

}
