using cakeslice;
using System.Collections;
using System.Collections.Generic;
using TC.GPConquest.Common;
using TC.GPConquest.Server;
using UnityEngine;

namespace TC.GPConquest.Player {

    public class OutlineTowerColorController : MonoBehaviour
    {

        public OutlineEffect OutLineEffect;
        public string FactionOwner = GameCommonNames.NO_FACTION;
        private string PreviousSelectedAction = "";

        //Keep in one place the color associated with each faction or actions like "capture of the tower"
        public static readonly Dictionary<string, Color> ColorsDictionary =
            new Dictionary<string, Color>()
            {
            { GameCommonNames.RED_FACTION,Color.red },
            { GameCommonNames.GREEN_FACTION, Color.green},
            { GameCommonNames.NO_FACTION, Color.cyan},
            { TowerCaptureController.STARTUP_STATE, Color.cyan},
            { TowerCaptureController.CAPTURE_IN_PROGRES, Color.yellow}
            };

        private void Awake()
        {
            OutLineEffect = GetComponent<OutlineEffect>();
            OutLineEffect.lineColor0 = ColorsDictionary[GameCommonNames.NO_FACTION];
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// This function is used for changing the outline color.
        /// Since that if you keep chaning color (e.g. in Update fuction),
        /// the outline component doesn't work anymore, we cache (PreviousSelectedAction)
        /// the previuous selection in order to avoid repeating changes.
        /// </summary>
        /// <param name="_actionName"></param>
        public void SetOutlineColor(string _actionName)
        {
            if (!ReferenceEquals(_actionName, null))
            {

                if (!PreviousSelectedAction.Equals(_actionName))
                {
                    PreviousSelectedAction = _actionName;
                    Color outColor;
                    ColorsDictionary.TryGetValue(_actionName, out outColor);
                    if (!ReferenceEquals(outColor, null))
                        OutLineEffect.lineColor0 = outColor;
                }

            }
            else Debug.LogError("Action given in input is null.");
        }
    }


}