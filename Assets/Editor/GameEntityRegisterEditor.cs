using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TC.Common;
using TC.GPConquest.Server;
using TC.GPConquest.Player;

namespace TC.GPConquest.GPConquestEditor{

    [CustomEditor(typeof(GameEntityRegister))]
    public class GameEntityRegisterEditor : Editor
    {

        public bool showTowers = false;
        public bool showPlayer = false;


        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            GameEntityRegister gameEditor = (GameEntityRegister)target;
            List<TowerEntityController> towersList = gameEditor.TowersList;
            List<DestinationController> playersList = gameEditor.DestinationsList;

            if (towersList.Count > 0)
            {
                GUIContent towerLabel = new GUIContent("Towers");
                showTowers = EditorGUILayout.Foldout(showTowers, towerLabel);

                if (showTowers)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NetworkId", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("GPS Coordinates", EditorStyles.boldLabel);
                    EditorGUILayout.EndHorizontal();

                    towersList.ForEach(x =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.IntField("", (int)x.networkObject.NetworkId);
                        GUIContent guiGPS = new GUIContent("");
                        Vector2 gpsCoords = new Vector2(x.GPSCoords.x, x.GPSCoords.y);
                        EditorGUILayout.Vector2Field(guiGPS, gpsCoords);
                        EditorGUILayout.EndHorizontal();
                    });
                }
            }

            if (playersList.Count > 0)
            {
                GUIContent playerLabel = new GUIContent("Players");
                showPlayer = EditorGUILayout.Foldout(showPlayer, playerLabel);

                if (showPlayer)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NetworkId", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Nickname", EditorStyles.boldLabel);
                    EditorGUILayout.EndHorizontal();

                    playersList.ForEach(x =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.IntField("", (int)x.networkObject.NetworkId);
                        GUIContent guiGPS = new GUIContent("");
                        EditorGUILayout.TextField(guiGPS, x.PlayerName);
                        EditorGUILayout.EndHorizontal();
                    });
                }
            }

        }
    }

}


