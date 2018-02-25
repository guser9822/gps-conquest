using MarkLight.Views.UI;
using UnityEngine.SceneManagement;
using System;
using TC.Common;

namespace TC.GPConquest.MarkLight4GPConquest
{
    public class LoginUI : UIView
    {
        public InputField InputUsername;
        public InputField InputPass;
        public Button LogInButton;

        public void CallConnectToServer()
        {
            SceneManager.LoadScene(GPCSceneManager.GetSceneIndex(GPCSceneManager.GPCSceneEnum.GAME_SCENE));
        }

        public void CallBack()
        {
            SceneManager.LoadScene(GPCSceneManager.GetSceneIndex(GPCSceneManager.GPCSceneEnum.CLIENT_MENU));
        }

    }
}
