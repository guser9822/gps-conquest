using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.GPConquest.Player
{
    public class GameUIController : MonoBehaviour
    {

        public GameObject PlayerUI;
        protected GameObject AvatorUIViewPresenter;
        public Camera Camera;

        public bool CreateGameUI(PlayerEntity _playerEntity)
        {
            AvatorUIViewPresenter = Instantiate<GameObject>(PlayerUI,
            new Vector3(transform.position.x, transform.position.y + 3, transform.position.z),
            Quaternion.identity);

            AvatorUIViewPresenter.transform.parent = transform;

            //Find the Camera in the scene so that the UI can refer to the correct camera
            //for player client and network clients
            Camera = FindObjectOfType<Camera>();

            AvatorUIViewPresenter.transform.rotation =
                Quaternion.RotateTowards(PlayerUI.transform.rotation, Camera.transform.rotation, 360);
            return true;
        }

    }
}


