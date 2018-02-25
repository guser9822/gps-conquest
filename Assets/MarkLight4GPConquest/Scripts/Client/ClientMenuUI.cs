using UnityEngine;
using MarkLight.Views.UI;
using UnityEngine.SceneManagement;
using TC.Common;

namespace TC.GPConquest.MarkLight4GPConquest
{
    public class ClientMenuUI : UIView
    {
        public void CallRegister()
        {

        }

        public void CallLogin()
        {
            SceneManager.LoadScene(GPCSceneManager.GetSceneIndex(GPCSceneManager.GPCSceneEnum.LOGIN));
        }

        public void CallOption()
        {

        }

        public void CallExit()
        {
            Application.Quit();
        }
    }
}

