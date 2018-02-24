using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;


namespace TC.GPConquest.MarkLight4GPConquest
{
    public class SelectUMAUI : UIView
    {

        public UMASelectionController UMASelectionController;

        public override void Initialize()
        {
            base.Initialize();
            UMASelectionController = FindObjectOfType<UMASelectionController>();
        }

        public void NextUMA()
        {
            UMASelectionController.ChangeUMA(UMASelectionController.VERSE.NEXT);
        }

        public void PrevUMA()
        {
            UMASelectionController.ChangeUMA(UMASelectionController.VERSE.PREV);
        }

    }

}
