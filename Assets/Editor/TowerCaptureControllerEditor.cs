using System.Collections;
using System.Collections.Generic;
using TC.GPConquest.Player;
using TC.GPConquest.Server;
using UnityEditor;
using UnityEngine;

namespace TC.GPConquest.GPConquestEditor
{
    [CustomEditor(typeof(TowerCaptureController))]
    public class TowerCaptureControllerEditor : Editor
    {
        public bool showPlayer = false;
        public override void OnInspectorGUI()
        {
            TowerCaptureController towerCaptureController = (TowerCaptureController)target;
            List<DestinationController> playersList = towerCaptureController.PlayerCapturingTowerList;

            if (playersList.Count > 0)
            {
                GUIContent playerLabel = new GUIContent("Players capturing the tower : ");
                showPlayer = EditorGUILayout.Foldout(showPlayer, playerLabel);
                if (showPlayer)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NetworkId", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Nickname", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Faction", EditorStyles.boldLabel);
                    EditorGUILayout.EndHorizontal();

                    playersList.ForEach(x =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.IntField("", (int)x.networkObject.NetworkId);
                        EditorGUILayout.TextField("", x.PlayerName);
                        EditorGUILayout.TextField("",x.AvatorController.PlayerEntity.faction);
                        EditorGUILayout.EndHorizontal();
                    });
                }
            }
        }
    }
}
