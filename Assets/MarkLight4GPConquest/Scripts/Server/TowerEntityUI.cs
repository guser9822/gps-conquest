using MarkLight;
using MarkLight.Views.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.GPConquest.MarkLight4GPConquest.Server
{
    /// <summary>
    /// This class manage the graphic interface of tower entities
    /// </summary>
    public class TowerEntityUI : UIView
    {
        public _string TowerFaction;
        public Window TowerUIWindow;

        public override void Initialize()
        {
            base.Initialize();
            TowerFaction.Value = "TEST";
        }

        public void Start()
        {
            ToggleWindow();
        }

        public void ToggleWindow()
        {
            if (TowerUIWindow.IsOpen)
            {
                TowerUIWindow.Close();
            }
            else
            {
                TowerUIWindow.Open();
            }
        }


    }
}
