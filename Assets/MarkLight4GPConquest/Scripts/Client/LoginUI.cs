using MarkLight.Views.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using TC.Common;
using TC.GPConquest.Player;
using TC.GPConquest.MarkLight4GPConquest.Common;

namespace TC.GPConquest.MarkLight4GPConquest
{
    public class LoginUI : UIView
    {
        public InputField InputUsername;
        public InputField InputPass;
        public Button LogInButton;

        public void CallConnectToServer()
        {
            UsersContainer userContainer = FindObjectOfType<UsersContainer>();         
            SceneManager.LoadScene(GPCSceneManager.GetSceneIndex(GPCSceneManager.GPCSceneEnum.GAME_SCENE));
        }

        public void CallBack()
        {
            SceneManager.LoadScene(GPCSceneManager.GetSceneIndex(GPCSceneManager.GPCSceneEnum.CLIENT_MENU));
        }

        private void Update()
        {
            ChecksOnUI();
        }

        protected void ChecksOnUI()
        {
            UIHelperUtility.ExecuteActionOnButton<String>(LogInButton,
                new List<String>() {InputUsername.Text.Value,InputPass.Text.Value},
                                    x => x.TrueForAll(y => UIHelperUtility.ValidateString(y)),
                                    (x, y) => y.IsVisible.Value = x);
        }

    }
}
