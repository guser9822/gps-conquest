using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using TC.GPConquest.MarkLight4GPConquest.Common;

namespace TC.GPConquest.MarkLight4GPConquest.Player
{
    public class PlayerUI : UIView
    {

        public Button MenuButton;
        public PlayerMenuWindow PlayerMenuWind;

        public void TogglePlayerMenuWindow()
        {
            PlayerMenuWind.ToggleWindow();
            ToggleMenuButtonVisibility();
        }

        public void ToggleMenuButtonVisibility()
        {
            UIHelperUtility.ExecuteActionOnButton<bool>(MenuButton,
                new List<bool> { MenuButton.IsVisible.Value },
                x => x.TrueForAll(y => y.Equals(!y)),
                (x, y) => y.IsVisible.Value = x);
        }

        public void OnClickCloseButton()
        {
            ToggleMenuButtonVisibility();
        }

    }
}
