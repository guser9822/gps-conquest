using MarkLight;
using MarkLight.Views.UI;
using System.Collections;
using System.Collections.Generic;
using TC.GPConquest.Common;
using UnityEngine;

namespace TC.GPConquest.MarkLight4GPConquest.Server {

    public class TowerEntityGameUI : UIView
    {

        public _string TowerFaction;
        public Window TowerUIWindow;

        public override void Initialize()
        {
            base.Initialize();
            TowerFaction.Value = "Faction Owner : "+GameCommonNames.NO_FACTION;
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

        public void ChangeTowerUIStatus(string _factionName)
        {
            if (!ReferenceEquals(_factionName, null) && _factionName.Length > 0)
            {
                TowerFaction.Value = "Faction Owner : "+_factionName;
            }
            else
            {
                Debug.LogError("Faction name cannot be null or empty. ");
            }
        }

    }

}
