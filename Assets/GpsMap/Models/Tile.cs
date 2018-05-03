﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Helpers;
using Assets.Models;
using UnityEngine;
using TC.Common;

namespace Assets
{
    public class Tile : MonoBehaviour
    {
        public Rect Rect;
        public Vector2 cen;
        Dictionary<Vector3, BuildingHolder> BuildingDictionary { get; set; }
        Dictionary<Vector3, WaterHolder> WaterDictionary { get; set; }
        Dictionary<Vector3, ParkHolder> ParkDictionary { get; set; }
        public bool DownlaodTiles;
        private float radiusCheckSphere;

        private void Awake()
        {
            DownlaodTiles = false;
        }

        public Tile()
        {
            //Dictionaries to hold buildings/water/parks
            BuildingDictionary = new Dictionary<Vector3, BuildingHolder>();
            WaterDictionary = new Dictionary<Vector3, WaterHolder>();
            ParkDictionary = new Dictionary<Vector3, ParkHolder>();
        }

        public IEnumerator CreateTile(Vector2 realPos, Vector2 worldCenter, int zoom)
        {
            Vector3 tp = transform.position;

            TileHelper.CheckNearbyTileResult collisionResult = TileHelper.CheckNearbyTile(tp, gameObject.name);

            if (collisionResult.IsCollided)
                cen = collisionResult.RealPosition;
            else cen = realPos;

            //Tile will try to create itself from nearby tiles, otherwise it will not
            //if (Physics.CheckSphere(tp + Vector3.right * 612, 0.5f))
            //{
            //    //Debug.Log("-r " + gameObject.name + " from " + Physics.OverlapSphere(tp + Vector3.right * 612, 0.5f)[0].name);
            //    //realPos = Physics.OverlapSphere(tp + Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen - Vector2.right;
            //    var collisions = Physics.OverlapSphere(tp + Vector3.right * 612, 0.5f);
            //    foreach (var coll in collisions)
            //    {
            //        if (coll.CompareTag(CommonNames.TILE_TAG))
            //        {
            //            var t = coll.gameObject.GetComponent<Tile>();
            //            Debug.Log("-r " + gameObject.name + " from " + t.name);
            //            realPos = t.cen - Vector2.right;
            //            break;
            //        }
            //    }
            //}
            //else if (Physics.CheckSphere(tp - Vector3.right * 612, 0.5f))
            //{
            //    //Debug.Log("+r " + gameObject.name + " from " + Physics.OverlapSphere(tp - Vector3.right * 612, 0.5f)[0].name);
            //    //realPos = Physics.OverlapSphere(tp - Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen + Vector2.right;
            //    var collisions = Physics.OverlapSphere(tp - Vector3.right * 612, 0.5f);
            //    foreach (var coll in collisions)
            //    {
            //        if (coll.CompareTag(CommonNames.TILE_TAG))
            //        {
            //            var t = coll.gameObject.GetComponent<Tile>();
            //            Debug.Log("+r " + gameObject.name + " from " + t.name);
            //            realPos = t.cen + Vector2.right;
            //            break;
            //        }
            //    }
            //}
            //else if (Physics.CheckSphere(tp + Vector3.forward * 612, 0.5f))
            //{
            //    //Debug.Log("+u " + gameObject.name + " from " + Physics.OverlapSphere(tp + Vector3.forward * 612, 0.5f)[0].name);
            //    //realPos = Physics.OverlapSphere(tp + Vector3.forward * 612, 0.5f)[0].GetComponent<Tile>().cen + Vector2.up;
            //    var collisions = Physics.OverlapSphere(tp + Vector3.forward * 612, 0.5f);
            //    foreach (var coll in collisions)
            //    {
            //        if (coll.CompareTag(CommonNames.TILE_TAG))
            //        {
            //            var t = coll.gameObject.GetComponent<Tile>();
            //            Debug.Log("+u " + gameObject.name + " from " + t.name);
            //            realPos = t.cen + Vector2.up;
            //            break;
            //        }
            //    }
            //}
            //else if (Physics.CheckSphere(tp - Vector3.forward * 612, 0.5f))
            //{
            //    //Debug.Log("-u " + gameObject.name + " from " + Physics.OverlapSphere(tp - Vector3.forward * 612, 0.5f)[0].name);
            //    //realPos = Physics.OverlapSphere(tp - Vector3.forward * 612, 0.5f)[0].GetComponent<Tile>().cen - Vector2.up;
            //    var collisions = Physics.OverlapSphere(tp - Vector3.forward * 612, 0.5f);
            //    foreach (var coll in collisions)
            //    {
            //        if (coll.CompareTag(CommonNames.TILE_TAG))
            //        {
            //            var t = coll.gameObject.GetComponent<Tile>();
            //            Debug.Log("-u  " + gameObject.name + " from " + t.name);
            //            realPos = t.cen - Vector2.up;
            //            break;
            //        }
            //    }
            //}
            ////diagonals
            ////honestly these should never really be called except for the beginning
            ////also may be somewhat broken atm
            //else if (Physics.CheckSphere(tp - Vector3.forward * 612 - Vector3.right * 612, 0.5f))
            //{
            //    //Debug.Log("--d " + gameObject.name + " from " + Physics.OverlapSphere(tp - Vector3.forward * 612 - Vector3.right * 612, 0.5f)[0].name);
            //    //realPos = Physics.OverlapSphere(tp - Vector3.forward * 612 - Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen - Vector2.up - Vector2.right;
            //    var collisions = Physics.OverlapSphere(tp - Vector3.forward * 612 - Vector3.right * 612, 0.5f);
            //    foreach (var coll in collisions)
            //    {
            //        if (coll.CompareTag(CommonNames.TILE_TAG))
            //        {
            //            var t = coll.gameObject.GetComponent<Tile>();
            //            Debug.Log("--d " + gameObject.name + " from " + t.name);
            //            realPos = t.cen - Vector2.up - Vector2.right;
            //            break;
            //        }
            //    }

            //}
            //else if (Physics.CheckSphere(tp - Vector3.forward * 612 + Vector3.right * 612, 0.5f))
            //{
            //    //Debug.Log("-+d " + gameObject.name + " from " + Physics.OverlapSphere(tp - Vector3.forward * 612 + Vector3.right * 612, 0.5f)[0].name);
            //    //realPos = Physics.OverlapSphere(tp - Vector3.forward * 612 + Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen - Vector2.up + Vector2.right;
            //    var collisions = Physics.OverlapSphere(tp - Vector3.forward * 612 + Vector3.right * 612, 0.5f);
            //    foreach (var coll in collisions)
            //    {
            //        if (coll.CompareTag(CommonNames.TILE_TAG))
            //        {
            //            var t = coll.gameObject.GetComponent<Tile>();
            //            Debug.Log("-+d " + gameObject.name + " from " + t.name);
            //            realPos = t.cen - Vector2.up + Vector2.right;
            //            break;
            //        }
            //    }
            //}
            //else if (Physics.CheckSphere(tp + Vector3.forward * 612 - Vector3.right * 612, 0.5f))
            //{
            //    //Debug.Log("+-d " + gameObject.name + " from " + Physics.OverlapSphere(tp + Vector3.forward * 612 - Vector3.right * 612, 0.5f)[0].name);
            //    //realPos = Physics.OverlapSphere(tp + Vector3.forward * 612 - Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen + Vector2.up - Vector2.right;
            //    var collisions = Physics.OverlapSphere(tp + Vector3.forward * 612 - Vector3.right * 612, 0.5f);
            //    foreach (var coll in collisions)
            //    {
            //        if (coll.CompareTag(CommonNames.TILE_TAG))
            //        {
            //            var t = coll.gameObject.GetComponent<Tile>();
            //            Debug.Log("+-d " + gameObject.name + " from " + t.name);
            //            realPos = t.cen + Vector2.up - Vector2.right;
            //            break;
            //        }
            //    }

            //}
            //else if (Physics.CheckSphere(tp + Vector3.forward * 612 + Vector3.right * 612, 0.5f))
            //{
            //    //Debug.Log("++d " + gameObject.name + " from " + Physics.OverlapSphere(tp + Vector3.forward * 612 + Vector3.right * 612, 0.5f)[0].name);
            //    //realPos = Physics.OverlapSphere(tp + Vector3.forward * 612 + Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen + Vector2.up - Vector2.right;
            //    var collisions = Physics.OverlapSphere(tp + Vector3.forward * 612 + Vector3.right * 612, 0.5f);
            //    foreach (var coll in collisions)
            //    {
            //        if (coll.CompareTag(CommonNames.TILE_TAG))
            //        {
            //            var t = coll.gameObject.GetComponent<Tile>();
            //            Debug.Log("++d " + gameObject.name + " from " + t.name);
            //            realPos = t.cen + Vector2.up - Vector2.right;
            //            break;
            //        }
            //    }
            //}

            //cen = realPos;

            //setting up the URL
            var tilename = Application.persistentDataPath + "/" + realPos.x + "_" + realPos.y;
            var tileurl = realPos.x + "/" + realPos.y;

            var url = "http://tile.mapzen.com/mapzen/vector/v1/water,earth,buildings,roads,landuse/" + zoom + "/";

            //Debug.Log(url);
            JSONObject mapData;

            //If the tile has been created in the past, load from memory
            //otherwise fetch from online and store a copy locally
            if (File.Exists(tilename))
            {
                var r = new StreamReader(tilename, Encoding.Default);
                mapData = new JSONObject(r.ReadToEnd());

                constructTiles(mapData, realPos, zoom, worldCenter);
                Debug.Log("Tile founded on the disk.");
                
            }
            else if(DownlaodTiles)
            {
                var www = new WWW(url + tileurl + ".json?api_key=mapzen-Mrq2fyY");
                yield return www;

                var sr = File.CreateText(tilename);
                sr.Write(www.text);
                sr.Close();

                mapData = new JSONObject(www.text);
                Debug.Log("Tile downloaded..");
                constructTiles(mapData, realPos, zoom, worldCenter);
            }

            //gameObject.AddComponent<BoxCollider>();

            //Rect = GM.TileBounds(realPos, zoom);

            ////make em
            //CreateBuildings(mapData["buildings"], worldCenter);
            //CreateWater(mapData["water"], worldCenter);
            //CreateParks(mapData["landuse"], worldCenter);
            //CreateRoads(mapData["roads"], worldCenter);
            
        }

        private void constructTiles(JSONObject mapData, Vector2 realPos, int zoom, Vector2 worldCenter) {

            /*
             * N.B We need to add a BoxCollider because when tileGen.cs  checks
             * if there's already a tile in this position (where with 'position' 
             * means the center of this tile) it uses a a sphere with a small radius 
             * for to check if there's a collider. 
             * ***/
            gameObject.AddComponent<BoxCollider>();
            Rect = GM.TileBounds(realPos, zoom);

            //make em
            CreateBuildings(mapData["buildings"], worldCenter);
            CreateWater(mapData["water"], worldCenter);
            CreateParks(mapData["landuse"], worldCenter);
            CreateRoads(mapData["roads"], worldCenter);

        }

        private void CreateBuildings(JSONObject mapData, Vector2 worldCenter)
        {
            //filter to just polygons
            foreach (var geo in mapData["features"].list.Where(x => x["geometry"]["type"].str == "Polygon"))
            {
                //convert and add points
                var l = new List<Vector3>();
                for (int i = 0; i < geo["geometry"]["coordinates"][0].list.Count - 1; i++)
                {
                    var c = geo["geometry"]["coordinates"][0].list[i];
                    var bm = GM.LatLonToMeters(c[1].f, c[0].f);
                    var pm = new Vector2(bm.x - Rect.center.x, bm.y - Rect.center.y);
                    l.Add(pm.ToVector3xz());
                }
                //make them buildings
                try
                {
                    var center = l.Aggregate((acc, cur) => acc + cur) / l.Count;
                    if (!BuildingDictionary.ContainsKey(center))
                    {
                        var bh = new BuildingHolder(center, l);
                        for (int i = 0; i < l.Count; i++)
                        {
                            l[i] = l[i] - bh.Center;
                        }
                        BuildingDictionary.Add(center, bh);

                        var m = bh.CreateModel();
                        m.name = "building";
                        m.transform.parent = this.transform;
                        center = new Vector3(center.x, center.y, center.z);
                        m.transform.localPosition = center;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

        private void CreateWater(JSONObject mapData, Vector2 worldCenter)
        {
            foreach (var geo in mapData["features"].list.Where(x => x["geometry"]["type"].str == "Polygon"))
            {
                var l = new List<Vector3>();
                for (int i = 0; i < geo["geometry"]["coordinates"][0].list.Count - 1; i++)
                {
                    var c = geo["geometry"]["coordinates"][0].list[i];
                    var bm = GM.LatLonToMeters(c[1].f, c[0].f);
                    var pm = new Vector2(bm.x - Rect.center.x, bm.y - Rect.center.y);
                    l.Add(pm.ToVector3xz());
                }

                try
                {
                    var center = l.Aggregate((acc, cur) => acc + cur) / l.Count;
                    if (!WaterDictionary.ContainsKey(center))
                    {
                        var bh = new WaterHolder(center, l);
                        for (int i = 0; i < l.Count; i++)
                        {
                            l[i] = l[i] - bh.Center;
                        }
                        WaterDictionary.Add(center, bh);

                        var m = bh.CreateModel();
                        m.name = "water";
                        m.transform.parent = this.transform;
                        center = new Vector3(center.x, center.y, center.z);
                        m.transform.localPosition = center;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

        private void CreateParks(JSONObject mapData, Vector2 worldCenter)
        {
            foreach (var geo in mapData["features"].list.Where(x => x["geometry"]["type"].str == "Polygon" && x["properties"]["kind"].str == "park"))
            {
                var l = new List<Vector3>();
                for (int i = 0; i < geo["geometry"]["coordinates"][0].list.Count - 1; i++)
                {
                    var c = geo["geometry"]["coordinates"][0].list[i];
                    var bm = GM.LatLonToMeters(c[1].f, c[0].f);
                    var pm = new Vector2(bm.x - Rect.center.x, bm.y - Rect.center.y);
                    l.Add(pm.ToVector3xz());
                }

                try
                {
                    var center = l.Aggregate((acc, cur) => acc + cur) / l.Count;
                    if (!WaterDictionary.ContainsKey(center))
                    {
                        var bh = new ParkHolder(center, l);
                        for (int i = 0; i < l.Count; i++)
                        {
                            l[i] = l[i] - bh.Center;
                        }
                        ParkDictionary.Add(center, bh);

                        var m = bh.CreateModel();
                        m.name = "park";
                        m.transform.parent = this.transform;
                        center = new Vector3(center.x, center.y, center.z);
                        m.transform.localPosition = center;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

        private void CreateRoads(JSONObject mapData, Vector2 worldCenter)
        {
            foreach (var geo in mapData["features"].list)
            {
                var l = new List<Vector3>();

                for (int i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                {
                    var c = geo["geometry"]["coordinates"][i];
                    var bm = GM.LatLonToMeters(c[1].f, c[0].f);
                    var pm = new Vector2(bm.x - Rect.center.x, bm.y - Rect.center.y);
                    l.Add(pm.ToVector3xz());
                }

                var m = new GameObject("road").AddComponent<RoadPolygon>();
                m.transform.parent = this.transform;
                try
                {
                    m.Initialize(geo["properties"]["id"].str, this, l, geo["properties"]["kind"].str);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

        public void Cleanup()
        {
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }
        }
    }

    public static class TileHelper {

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

        public struct CheckNearbyTileResult {

            public bool IsCollided;
            public Vector2 RealPosition;
            public TileOrientationTable TileOrientationCollided;

            public CheckNearbyTileResult(bool _isCollided=false) {
                IsCollided = _isCollided;
                RealPosition = Vector2.zero;
                TileOrientationCollided = TileOrientationTable.NOT_INTERESTING_VALUE;
            }

        }

        public static CheckNearbyTileResult CheckNearbyTile(Vector3 _tilePosition,string _tileName)
        {
            var checkResult = new CheckNearbyTileResult();
            Vector3 _positionOfCollision = Vector3.zero;
            foreach (var orientation in Enum.GetValues(typeof(TileOrientationTable)))
            {
                var tileOrien = (TileOrientationTable)orientation;
                _positionOfCollision = CalculateCollisionPoint(tileOrien, _tilePosition);

                if (!tileOrien.Equals(TileOrientationTable.NOT_INTERESTING_VALUE) && 
                    Physics.CheckSphere(_positionOfCollision, 0.5f))
                {
                    checkResult.IsCollided = true;
                    checkResult.TileOrientationCollided = tileOrien;
                    break;
                }
            }

            if (checkResult.IsCollided)
            {
                var collisions = Physics.OverlapSphere(_positionOfCollision, 0.5f);
                foreach (var coll in collisions)
                {
                    if (coll.CompareTag(CommonNames.TILE_TAG))
                    {
                        var t = coll.gameObject.GetComponent<Tile>();
                        Debug.Log("-r " + _tileName + " from " + t.name);
                        checkResult.RealPosition = CalculateRealPosition(checkResult.TileOrientationCollided, t.cen);
                        break;
                    }
                }
            }
        
            return checkResult;
        }

        private static Vector3 CalculateCollisionPoint(TileOrientationTable _tileOrientation,Vector3 _tilePosition)
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
            Vector2 realPos = new Vector2(_collTileCenter.x, _collTileCenter.y);
            switch (_tileOrientation)
            {
                case TileOrientationTable.NOT_INTERESTING_VALUE:
                    break;
                case TileOrientationTable.MINUS_R:
                    realPos -= Vector2.right;
                    break;
                case TileOrientationTable.PLUS_R:
                    realPos += Vector2.right;
                    break;
                case TileOrientationTable.MINUS_U:
                    realPos -= Vector2.up;
                    break;
                case TileOrientationTable.PLUS_U:
                    realPos += Vector2.up;
                    break;
                case TileOrientationTable.MINUS_MINUS_D:
                    realPos -= Vector2.up - Vector2.right;
                    break;
                case TileOrientationTable.MINUS_PLUS_D:
                    realPos -= Vector2.up + Vector2.right;
                    break;
                case TileOrientationTable.PLUS_MINUS_D:
                    realPos += Vector2.up - Vector2.right;
                    break;
                case TileOrientationTable.PLUS_PLUS_D:
                    realPos += Vector2.up - Vector2.right;
                    break;
            }
            return realPos;
        }

    }
}
