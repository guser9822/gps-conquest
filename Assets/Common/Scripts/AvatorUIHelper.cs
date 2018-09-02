using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TC.GPConquest.Server;
using UnityEngine;

namespace TC.Common.Helper
{

    public static class AvatorUIHelper
    {
        //TODO : This function must be moved somewhere else
        /// <summary>
        /// Find the nearest tower to the transform given in input that is not captured
        /// by no faction
        /// </summary>
        /// <param name="_gameEntityRegister"></param>
        /// <param name="_originPosition"></param>
        /// <returns>The nearest tower entity controller found</returns>
        public static TowerEntityController FindNearestTower(GameEntityRegister _gameEntityRegister,
            Vector3 _originPosition)
        {
            TowerEntityController res = null;
            if (!ReferenceEquals(_gameEntityRegister, null) && !ReferenceEquals(_originPosition, null))
            {
                var allTowersReg = _gameEntityRegister.GetAllEntity(typeof(TowerEntityController));
                var allTowers = allTowersReg.Cast<TowerEntityController>();

                res = allTowers.Where<TowerEntityController>(x => !x.IsTowerCaptured()).
                    OrderBy<TowerEntityController, double>(x => Vector3.Distance(_originPosition, x.transform.position)).
                    FirstOrDefault<TowerEntityController>();
            }
            else Debug.LogError("No game entity register supplied.");
            return res;
        }
    }

}