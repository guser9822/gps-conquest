using MarkLight.Views.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.GPConquest.MarkLight4GPConquest.Player
{

    public class PlayerMenuWindow : UIView
    {
        public Window BasePlayerMenuWindow;
        public Button StatsButton;
        public Button ExitButton;

        // Use this for initialization
        void Start()
        {
            ToggleWindow();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ToggleWindow()
        {
            if (BasePlayerMenuWindow.IsOpen)
            {
                BasePlayerMenuWindow.Close();
            }
            else
            {
                BasePlayerMenuWindow.Open();
            }
        }

    }

}
