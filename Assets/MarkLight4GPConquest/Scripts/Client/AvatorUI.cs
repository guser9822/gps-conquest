﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MarkLight.Views.UI;
using TC.GPConquest.Player;
using MarkLight;
using TC.Common;
using TC.GPConquest.Server;
using TC.Common.Helper;

namespace TC.GPConquest.MarkLight4GPConquest.Player
{
    /// <summary>
    /// This UI is used for all the stuffs regarding the character of the player (eg. it's nickname)
    /// </summary>
    public class AvatorUI : UIView
    {

        public Label NicknameLabel;
        [HideInInspector]
        public Camera CameraOnDestination;
        private PlayerEntity PlayerEntity;
        [MapTo("NicknameLabel.Text")]
        public _string UsernameText;
        private bool SetUpReady;
        protected bool IsCompassOwner;//flag for distinguish if the code is executed by the owner of the network object
        protected Transform CompassGameObject;//Game object used for the rotation of the compass
        protected Transform CompassItem;// model of the compass
        protected GameEntityRegister GameEntityRegister;

        #region AVATOR_UI SETTINGS
        public int HIDE_DISTANCE_COMPASS = 5;
        #endregion

        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Initialize AvatorUI.
        /// </summary>
        /// <param name="_cameraOnDestination"></param>
        /// <param name="_playerEntity"></param>
        /// <param name="_compass"></param>
        public void InitAvatorUI(Camera _cameraOnDestination, 
            PlayerEntity _playerEntity,
            Transform _compass)
        {
            CameraOnDestination = _cameraOnDestination;
            PlayerEntity = _playerEntity;
            UsernameText.Value =  _playerEntity.username;
            IsCompassOwner = _compass.gameObject.activeInHierarchy;

            if (IsCompassOwner)
            {
                CompassGameObject = _compass;
                CompassItem = CompassGameObject.GetChild(0);
                GameEntityRegister = _playerEntity.GameEntityRegister;
            }

            SetUpReady = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!ReferenceEquals(CameraOnDestination,null) && SetUpReady)
            {
                NicknameLabel.transform.rotation =
                     Quaternion.RotateTowards(NicknameLabel.transform.rotation,
                     CameraOnDestination.transform.rotation, 360);

                if (IsCompassOwner)
                {
                    AdjustCompassDirection();
                }

            }
        }

        /// <summary>
        /// Make the compass point in the right direction. If the player is too near the nearest tower, deactivae it
        /// </summary>
        protected void AdjustCompassDirection() {
            var nearestTower = AvatorUIHelper.FindNearestTower(GameEntityRegister,transform.position);
            if (!ReferenceEquals(nearestTower, null))
            {
                var nearesTTowerTransform = nearestTower.transform;
                if (Vector3.Distance(transform.position, nearesTTowerTransform.position) >= HIDE_DISTANCE_COMPASS)
                {
                    CompassGameObject.gameObject.SetActive(true);
                    CompassGameObject.LookAt(nearesTTowerTransform);
                    CompassItem.LookAt(nearesTTowerTransform);
                }
                else {
                    /* Hide the compass when the player is too near the tower*/
                    CompassGameObject.gameObject.SetActive(false);
                }
            }
        }

    }
}
