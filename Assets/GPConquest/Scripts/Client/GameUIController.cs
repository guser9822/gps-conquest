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
        public AvatorUI AvatorUI;

        //This function initialize the GameUI. isTheOwnerOnNetwork states if the caller
        //of this function it's the client owner of the object on the network
        public bool InitializeGameUI(Transform _parentTransform,
            Camera _camerOnDestination,
            bool isTheOwnerOnNetwork = false)
        {
            //Instantiates the GameUI
            AvatorUIViewPresenter = Instantiate<GameObject>(PlayerUI);

            CameraOnDestination = _camerOnDestination;

            //Puts the UI under the hieararchy of the GameUIController object
            AvatorUIViewPresenter.transform.SetParent(_parentTransform);

            //Sets the correct position for the UI
            AvatorUIViewPresenter.transform.localPosition =
                new Vector3(0.0f,
                0,
                0f);

            //Gets the AvatorUI and initialize it with the camera that it must follow
            AvatorUI = AvatorUIViewPresenter.GetComponentInChildren<AvatorUI>();
            AvatorUI.AvatorUIInitializator(CameraOnDestination);

            //if (!isTheOwnerOnNetwork)
            //{
            //    //Deactivates static parts of the UI
            //    var playerUI = AvatorUIViewPresenter.GetComponentInChildren<PlayerUI>();
            //    playerUI.gameObject.SetActive(false);
            //    //Deactivates the EventSystem
            //    var eventSystem = AvatorUIViewPresenter.GetComponentInChildren<EventSystem>();
            //    eventSystem.gameObject.SetActive(false);
            //}

            canUpdate = true;

            return canUpdate;
        }

        private void Update()
        {

        }

    }
}


