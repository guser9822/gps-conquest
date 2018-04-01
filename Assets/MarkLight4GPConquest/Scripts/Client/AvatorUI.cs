using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkLight.Views.UI;

namespace TC.GPConquest.MarkLight4GPConquest.Player
{
    public class AvatorUI : UIView
    {

        public Label NicknameLabel;
        private Camera CameraOnDestination = null;

        // Use this for initialization
        void Start()
        {
            NicknameLabel.transform.localPosition = new Vector3(0, 220, 0);
        }

        // This method must be called with the appropriate parameters in order to
        // ensure correct work of the AvatorUI
        public void AvatorUIInitializator(Camera _cameraOnDestination) {
            CameraOnDestination = _cameraOnDestination;
        }

        // Update is called once per frame
        void Update()
        {
            if (CameraOnDestination != null)
            {
               NicknameLabel.transform.rotation =
                    Quaternion.RotateTowards(NicknameLabel.transform.rotation, 
                    CameraOnDestination.transform.rotation, 360);
            }
        }


    }
}
