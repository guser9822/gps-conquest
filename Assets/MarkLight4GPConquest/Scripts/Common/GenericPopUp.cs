﻿using System.Collections;
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
        public Button CancelButton;

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

        public void ShowPopUp(string _message,bool showOkButton = true,bool showCancelButton = false)
        {
            ToggleWindow();
            OkButton.IsActive.Value = showOkButton;
            CancelButton.IsActive.Value = showCancelButton;
            GenericPopUpMessage.Value = _message;
        }

    }

}