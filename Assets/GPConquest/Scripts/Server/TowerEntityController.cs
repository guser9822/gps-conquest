using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;

namespace TC.GPConquest.Server {

    public class TowerEntityController : TowerEntityControllerBehavior,IEqualityComparer<TowerEntityController>
    {

        public string ownerFaction;

        public void InitTowerEntityController(Vector2 _GPSCoords)
        {
            if (!networkObject.IsOwner) return;


        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void UpdateTowerAttrributes(RpcArgs args)
        {
            ownerFaction = args.GetNext<string>();
        }

        public bool Equals(TowerEntityController x, TowerEntityController y)
        {
            return x.networkObject.NetworkId == y.networkObject.NetworkId &&
                x.networkObject.towerGPSCoords == y.networkObject.towerGPSCoords;
        }

        public int GetHashCode(TowerEntityController obj)
        {
            int result = 89;
            result = 13 * result + obj.networkObject.NetworkId.GetHashCode();
            result = 13 * result + obj.networkObject.towerGPSCoords.GetHashCode();
            return result;
        }
    }
}
