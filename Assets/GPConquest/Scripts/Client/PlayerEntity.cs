using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;

namespace TC.GPConquest.Player
{
    /*
     * This class is used to manage the player information in the network
     * **/
    public class PlayerEntity : PlayerEntityModelBehavior
    {

        public string username;
        public string password;
        public string email;
        public string faction;
        public string selectedUma;

        public bool InitializePlayerEntity(Transform parentTransform, UserInformations _user)
        {
            if (!networkObject.IsOwner) return false;

            transform.SetParent(parentTransform);

            networkObject.SendRpc(RPC_UPDATE_PLAYER_ENTITY,
                Receivers.AllBuffered,
            _user.username,
            _user.password,
            _user.email,
            _user.faction,
            _user.selectedUma);

            return true;
        }

        public override void UpdatePlayerEntity(RpcArgs args)
        {
           username = args.GetNext<string>();
           password = args.GetNext<string>();
           email = args.GetNext<string>();
           faction = args.GetNext<string>();
           selectedUma = args.GetNext<string>();
        }
    }
}
