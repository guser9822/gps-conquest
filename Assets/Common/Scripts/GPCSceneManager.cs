using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.Common
{
    /*
     * This class is used for managin all the scene on the client side.
     * Each constant in the enumeration is aligned with the building 
     * index choosen in the Build section in the Unity Editor.
     * Use the method GetSceneIndex in order to get the
     * index of the scene, given the enum name.
     * **/
    public static class GPCSceneManager
    {
        public enum GPCSceneEnum
        {
            CLIENT_MENU,
            REGISTER,
            SELECT_UMA,
            LOGIN,
            GAME_SCENE,
            OPTION,
        }

        public static int GetSceneIndex(GPCSceneEnum _scene)
        {
            return (int)_scene;
        }

    }

}