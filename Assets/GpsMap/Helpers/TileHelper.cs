using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using TC.Common;
using UnityEngine;
using System.Linq;


namespace TC.GPConquest.GpsMap.Helpers{

    public static class TileHelper
    {

        public static CheckNearbyTileResult CheckNearbyTile(Vector3 _tilePosition, string _tileName)
        {
            var checkResult = new CheckNearbyTileResult();
            Vector3 _positionOfCollision = Vector3.zero;

            checkResult.IsCollided = Enum.GetValues(typeof(TileOrientationTable)).
                Cast<TileOrientationTable>().
                Any<TileOrientationTable>(
                    tileOrien =>
                    {
                        _positionOfCollision = CalculateCollisionPoint(tileOrien, _tilePosition);
                        if (!tileOrien.Equals(TileOrientationTable.NOT_INTERESTING_VALUE) &&
                            Physics.CheckSphere(_positionOfCollision, 0.5f))
                        {
                            checkResult.TileOrientationCollided = tileOrien;
                            return true;
                        }
                    return false;
                });

            if (checkResult.IsCollided)
            {
                var tile = FindTile(_positionOfCollision, 0.5f);
                if (!ReferenceEquals(tile, null))
                {
                    Debug.Log(checkResult.TileOrientationCollided.ToString() + " " + _tileName + " from " + tile.name);
                    checkResult.RealPosition = CalculateRealPosition(checkResult.TileOrientationCollided, tile.cen);
                }
            }

            return checkResult;
        }

        private static Vector3 CalculateCollisionPoint(TileOrientationTable _tileOrientation, Vector3 _tilePosition)
        {
            Vector3 tp = new Vector3(_tilePosition.x, _tilePosition.y, _tilePosition.z);
            switch (_tileOrientation)
            {
                case TileOrientationTable.NOT_INTERESTING_VALUE:
                    break;
                case TileOrientationTable.MINUS_R:
                    tp += Vector3.right * 612;
                    break;
                case TileOrientationTable.PLUS_R:
                    tp -= Vector3.right * 612;
                    break;
                case TileOrientationTable.MINUS_U:
                    tp -= Vector3.forward * 612;
                    break;
                case TileOrientationTable.PLUS_U:
                    tp += Vector3.forward * 612;
                    break;
                case TileOrientationTable.MINUS_MINUS_D:
                    tp -= Vector3.forward * 612 - Vector3.right * 612;
                    break;
                case TileOrientationTable.MINUS_PLUS_D:
                    tp -= Vector3.forward * 612 + Vector3.right * 612;
                    break;
                case TileOrientationTable.PLUS_MINUS_D:
                    tp += Vector3.forward * 612 - Vector3.right * 612;
                    break;
                case TileOrientationTable.PLUS_PLUS_D:
                    tp += Vector3.forward * 612 + Vector3.right * 612;
                    break;
            }
            return tp;
        }

        private static Vector2 CalculateRealPosition(TileOrientationTable _tileOrientation, Vector2 _collTileCenter)
        {
            Vector2 calcCen = new Vector2(_collTileCenter.x, _collTileCenter.y);
            switch (_tileOrientation)
            {
                case TileOrientationTable.NOT_INTERESTING_VALUE:
                    break;
                case TileOrientationTable.MINUS_R:
                    calcCen -= Vector2.right;
                    break;
                case TileOrientationTable.PLUS_R:
                    calcCen += Vector2.right;
                    break;
                case TileOrientationTable.MINUS_U:
                    calcCen -= Vector2.up;
                    break;
                case TileOrientationTable.PLUS_U:
                    calcCen += Vector2.up;
                    break;
                case TileOrientationTable.MINUS_MINUS_D:
                    calcCen -= Vector2.up - Vector2.right;
                    break;
                case TileOrientationTable.MINUS_PLUS_D:
                    calcCen -= Vector2.up + Vector2.right;
                    break;
                case TileOrientationTable.PLUS_MINUS_D:
                    calcCen += Vector2.up - Vector2.right;
                    break;
                case TileOrientationTable.PLUS_PLUS_D:
                    calcCen += Vector2.up - Vector2.right;
                    break;
            }
            return calcCen;
        }

        /*
        * Get the first tile, if exists, that collide with the given position and ray else null
        * **/
        public static Tile FindTile(Vector3 _positionOfCollision, float _ray)
        {
            var collisions = Physics.OverlapSphere(_positionOfCollision, _ray);
            var tileCollider = Enumerable.ToList<Collider>(collisions).FirstOrDefault<Collider>(x =>
            {
                return x.CompareTag(CommonNames.TILE_TAG);
            });
            return ReferenceEquals(tileCollider, null) ? null : tileCollider.gameObject.GetComponent<Tile>();
        }

        public enum TileOrientationTable
        {
            NOT_INTERESTING_VALUE,
            MINUS_R,
            PLUS_R,
            MINUS_U,
            PLUS_U,
            MINUS_MINUS_D,
            MINUS_PLUS_D,
            PLUS_MINUS_D,
            PLUS_PLUS_D
        }

        public struct CheckNearbyTileResult
        {

            public bool IsCollided;
            public Vector2 RealPosition;
            public TileOrientationTable TileOrientationCollided;

            public CheckNearbyTileResult(bool _isCollided = false)
            {
                IsCollided = _isCollided;
                RealPosition = Vector2.zero;
                TileOrientationCollided = TileOrientationTable.NOT_INTERESTING_VALUE;
            }

        }

    }

}
