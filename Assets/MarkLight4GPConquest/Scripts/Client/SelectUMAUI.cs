using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using TC.GPConquest.Player;
using MarkLight;
using TC.GPConquest.Common;

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
        public Region UserDataRegion;
        public Group UserDataGroup;

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
            UmaSelectionController.ChangeUMA(UMASelectionController.VERSE.NEXT);
        }

        public void PrevUMA()
        {
            UmaSelectionController.ChangeUMA(UMASelectionController.VERSE.PREV);
        }

    }

}
