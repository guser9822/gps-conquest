using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;


namespace TC.GPConquest {

    public class GameStatusController : MonoBehaviour
    {

        private void Start()
        {
            //Instantiate Player
            NetworkManager.Instance.InstantiatePlayerDestinationController(0);
        }
    }

}
