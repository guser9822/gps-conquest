using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using MarkLight;

namespace TC.GPConquest.MarkLight4GPConquest
{
    [HideInPresenter]
    public class GenericPopUp : UIView
    {
        public Window GenericWindow;
        public _string GenericPopUpMessage;
        public Button OkButton;

        public void Start()
        {
            ToggleWindow();
        }

        public void ToggleWindow()
        {
            if (GenericWindow.IsOpen)
            {
                GenericWindow.Close();
            }
            else
            {
                GenericWindow.Open();
            }
        }

        public void ShowPopUp(string _message)
        {
            GenericPopUpMessage.Value = _message;
            ToggleWindow();
        }

    }

}