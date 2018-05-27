using MarkLight.Views.UI;
using System.Collections;
using System.Collections.Generic;
using TC.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TC.GPConquest.MarkLight4GPConquest
{
    public class ServerAndClientUI : UIView
    {
        public void CallServer()
        {
            Debug.Log("LOL");
            SceneManager.LoadScene(GPCSceneManager.GetSceneIndex(GPCSceneManager.GPCSceneEnum.SERVER_SCENE));
        }

        public void CallClient()
        {
            SceneManager.LoadScene(GPCSceneManager.GetSceneIndex(GPCSceneManager.GPCSceneEnum.CLIENT_MENU));
        }

        public void CallExit()
        {
            Application.Quit();
        }
    }
}


