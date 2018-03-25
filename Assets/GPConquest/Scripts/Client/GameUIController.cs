using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public bool InitializeGameUI(bool isTheOwnerOnNetwork,Camera _camerOnDestination)
        {
            //Instantiates the GameUI
            AvatorUIViewPresenter = Instantiate<GameObject>(PlayerUI,
                new Vector3(transform.position.x, transform.position.y + 2, transform.position.z),
                Quaternion.identity);

            CameraOnDestination = _camerOnDestination;

            //Puts the UI under the hieararchy of the GameUIController object
            AvatorUIViewPresenter.transform.SetParent(transform);

            //Rotates the UI towards the player camera
            AvatorUIViewPresenter.transform.rotation =
                Quaternion.RotateTowards(PlayerUI.transform.rotation, CameraOnDestination.transform.rotation, 360);

            if (!isTheOwnerOnNetwork)
            {
                //Deactivates static parts of the UI
                //AvatorUIViewPresenter.GetComponentInChildren<EventSy>
                //Deactivates the EventSystem
            }

            canUpdate = true;

            return canUpdate;
        }

        private void Update()
        {
            if(canUpdate)
                AvatorUIViewPresenter.transform.rotation =
                    Quaternion.RotateTowards(PlayerUI.transform.rotation, CameraOnDestination.transform.rotation, 360);
        }

    }
}


