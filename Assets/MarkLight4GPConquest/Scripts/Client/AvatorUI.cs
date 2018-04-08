using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;
using TC.GPConquest.Player;
using MarkLight;

namespace TC.GPConquest.MarkLight4GPConquest.Player
{
    public class AvatorUI : UIView
    {

        public Label NicknameLabel;
        private Camera CameraOnDestination = null;
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
            UsernameText.Value =  PlayerEntity.username;
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
