using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.Common {

    //This class is used to convert between Unity coordinates system and GPS coordinates system
    public static class GPSHelper
    {
        //Level of detail of the pyramids of the tiles
        public static readonly int Zoom = 16;

        //The tile size is 256 x 256 pixels which corresponds to 611 Unity units
        private static readonly int _256pxToUnityUnits = 611;

        private static Vector2 LatLongToXY(float lat, float lng) {
            float n = Mathf.Pow(2, Zoom);
            float xtile = n * ((lng + 180) / 360);
            float ytile = n * (1 - (Mathf.Log(Mathf.Tan(Mathf.Deg2Rad * lat) +
                                (1f / Mathf.Cos(Mathf.Deg2Rad * lat))) / Mathf.PI)) / 2f;
            return new Vector2(xtile,ytile);
        }

        public static Vector2 CalcTile(float lat, float lng)
        {
            var XYcoords = LatLongToXY(lat, lng);
            return new Vector2((int)XYcoords.x, (int)XYcoords.y);
        }

        public static Vector2 PosInTile(float lat, float lng)
        {
            var XYcoords = LatLongToXY(lat, lng);
            return new Vector2(XYcoords.x - (int)XYcoords.x, XYcoords.y - (int)XYcoords.y);
        }

        public static Vector3 LatLongToUnityCoords(float lat, float lng) {
            var latLongToTile = PosInTile(lat, lng);
            return new Vector3((latLongToTile.x - 0.5f) * _256pxToUnityUnits, 0, (0.5f - latLongToTile.y) * _256pxToUnityUnits);
        }
    }

}