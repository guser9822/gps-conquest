using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using TC.GPConquest.Player;
using MarkLight;

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
        private bool canUpdate;

        public override void Initialize()
        {
            base.Initialize();
            NicknameLabel.Position.Value = new Vector3(0, 220, 0);
        }

        // This method must be called with the appropriate parameters in order to
        // ensure correct work of the AvatorUI
        public void InitAvatorUI(Camera _cameraOnDestination, PlayerEntity _playerEntity)
        {
            CameraOnDestination = _cameraOnDestination;
            PlayerEntity = _playerEntity;
            UsernameText.Value =  _playerEntity.username;
            canUpdate = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (CameraOnDestination != null && canUpdate)
            {
                NicknameLabel.transform.rotation =
                     Quaternion.RotateTowards(NicknameLabel.transform.rotation,
                     CameraOnDestination.transform.rotation, 360);

                NicknameLabel.RectTransform.localPosition = new Vector3(0, 220, 0);

            }
        }


    }
}
