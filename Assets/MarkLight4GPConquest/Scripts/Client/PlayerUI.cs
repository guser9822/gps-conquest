using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using TC.GPConquest.MarkLight4GPConquest.Common;
using TC.GPConquest.Player;

namespace TC.GPConquest.MarkLight4GPConquest.Player
{
    public class PlayerUI : UIView
    {

        public Button MenuButton;
        public PlayerMenuWindow PlayerMenuWind;
        private DestinationController DestinationController;

        public void InitPlayerUI(AvatorController _avatorControllerReference)
        {
            DestinationController = _avatorControllerReference.DestinationControllerReference;
        }

        public void TogglePlayerMenuWindow()
        {
            PlayerMenuWind.ToggleWindow();
            ToggleMenuButtonVisibility();
        }

        public void ToggleMenuButtonVisibility()
        {
            MenuButton.IsVisible.Value = MenuButton.IsVisible.Value ? false : true; 
        }

        public void OnClickCloseMenuButton()
        {
            ToggleMenuButtonVisibility();
        }

        public void OnClickExitButton()
        {
            DestinationController.DestroyDestinationController();
        }

    }
}
