using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.Common
{
    /*
     * This class is used for managing all the scenes-
     * Each constant in the enumeration is aligned with the building 
     * index choosen in the Build section in the Unity Editor.
     * Use the method GetSceneIndex in order to get the
     * index of the scene, given the enum name.
     * Note, the reason why the Enum starts with 1 is that
     * at index 0 there's the ServerAndClient scene which
     * must not be presente in this enumeration.
     * **/
    public static class GPCSceneManager
    {
        public enum GPCSceneEnum
        {
            SERVER_SCENE = 1,
            CLIENT_MENU,
            REGISTER,
            LOGIN,
            GAME_SCENE,
            OPTION
        }

        public static int GetSceneIndex(GPCSceneEnum _scene)
        {
            return (int)_scene;
        }

    }

}