using UnityEngine;
using MarkLight.Views.UI;
using UnityEngine.SceneManagement;

namespace TC.GPConquest.MarkLight4GPConquest
{
    public class ClientMenuUI : UIView
    {
        public void CallRegister()
        {

        }

        public void CallLogin()
        {
            SceneManager.LoadScene(/*SceneManager.GetActiveScene().buildIndex + 1*/3);
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

