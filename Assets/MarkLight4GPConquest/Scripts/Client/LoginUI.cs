using MarkLight.Views.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using TC.Common;
using TC.GPConquest.Player;
using TC.GPConquest.MarkLight4GPConquest.Common;
using TC.GPConquest.MarkLight4GPConquest;


namespace TC.GPConquest.MarkLight4GPConquest
{
    public class LoginUI : UIView
    {
        public InputField InputUsername;
        public InputField InputPass;
        public Button LogInButton;
        public GenericPopUp GenericPopUp;

        public void CallConnectToServer()
        {
            UsersContainer userContainer = FindObjectOfType<UsersContainer>();

            //Checks if the choosen account exists
            UserInformations u = userContainer.GetUserByUsernameAndPassword(InputUsername.Text.Value,
                InputPass.Text.Value);

            //If the account exists login in into the game scene
            if (!ReferenceEquals(u,null))
            {
                SceneManager.LoadScene(GPCSceneManager.GetSceneIndex(GPCSceneManager.GPCSceneEnum.GAME_SCENE));
            }else{
                GenericPopUp.ShowPopUp(UIInfoLayer.AccountNotFoundMessage);
            }

        }

        public void OnClickConfirm()
        {
            InputUsername.Text.Value = "";
            InputPass.Text.Value = "";
            GenericPopUp.ToggleWindow();
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
