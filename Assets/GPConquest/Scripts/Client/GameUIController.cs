using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MarkLight.Views;
using TC.GPConquest.MarkLight4GPConquest.Player;

namespace TC.GPConquest.Player
{
    public class GameUIController : MonoBehaviour
    {

        public GameObject PlayerUI;//prefabs of the UI, used only for instantiation
        protected GameObject AvatorUIViewPresenter;//instantiated UI
        public Camera CameraOnDestination;//camera of the DestinationController object
        private bool canUpdate;

        //This function initialize the GameUI. isTheOwnerOnNetwork states if the caller
        //of this function it's the client owner of the object on the network
        public bool InitializeGameUI(Transform _parentTransform,
            Camera _camerOnDestination,
            bool isTheOwnerOnNetwork = false)
        {
            //Instantiates the GameUI
            AvatorUIViewPresenter = Instantiate<GameObject>(PlayerUI,
                new Vector3(transform.position.x, transform.position.y + 2, transform.position.z),
                Quaternion.identity);

            CameraOnDestination = _camerOnDestination;

            //Puts the UI under the hieararchy of the GameUIController object
            AvatorUIViewPresenter.transform.SetParent(_parentTransform);

            AvatorUIViewPresenter.transform.position = new Vector3(0f,2.5f,0.4f);

            //Rotates the UI towards the player camera
            AvatorUIViewPresenter.transform.rotation =
                Quaternion.RotateTowards(PlayerUI.transform.rotation, CameraOnDestination.transform.rotation, 360);

            if (!isTheOwnerOnNetwork)
            {
                //Deactivates static parts of the UI
                var playerUI = AvatorUIViewPresenter.GetComponentInChildren<PlayerUI>();
                playerUI.gameObject.SetActive(false);
                //Deactivates the EventSystem
                var eventSystem = AvatorUIViewPresenter.GetComponentInChildren<EventSystem>();
                eventSystem.gameObject.SetActive(false);
            }

            canUpdate = true;

            return canUpdate;
        }

        private void Update()
        {
            if (canUpdate)
                AvatorUIViewPresenter.transform.rotation =
                    Quaternion.RotateTowards(PlayerUI.transform.rotation, CameraOnDestination.transform.rotation, 360);
        }

    }
}


