using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;


namespace TC.GPConquest {

    public class GameStatusController : MonoBehaviour
    {

        private void Start()
        {
            NetworkManager.Instance.InstantiatePlayerDestinationController(0);
        }
    }

}
