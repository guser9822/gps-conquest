using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;

namespace TC.GPConquest.Server {

    public class TowerEntityController : TowerEntityControllerBehavior
    {

        public string ownerFaction;

        protected override void NetworkStart()
        {
            base.NetworkStart();
            InitTowerEntityController();
        }

        protected void InitTowerEntityController()
        {
            
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
    }
}
