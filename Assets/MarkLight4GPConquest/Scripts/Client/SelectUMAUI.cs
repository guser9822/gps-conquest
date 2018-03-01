using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using TC.GPConquest.Player;

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
         * scripts for you, just declaring them as class's properties.cThis 
         * will also make more simple every dipendecy problem and will also 
         * avoid use of the methods like GameObject.FindObjectOfType<>().
         * */
        public UMASelectionController UmaSelectionController;
        public AssetLoaderController AssetLoaderController;

        public override void Initialize()
        {
            base.Initialize();
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
