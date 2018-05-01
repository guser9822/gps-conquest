using System;
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
            //realPos = TileHelper.CheckNearbyTile(tp, realPos, gameObject.name);
            ////Tile will try to create itself from nearby tiles, otherwise it will not
            if (Physics.CheckSphere(tp + Vector3.right * 612, 0.5f))
            {
                //Debug.Log("-r " + gameObject.name + " from " + Physics.OverlapSphere(tp + Vector3.right * 612, 0.5f)[0].name);
                //realPos = Physics.OverlapSphere(tp + Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen - Vector2.right;
                var collisions = Physics.OverlapSphere(tp + Vector3.right * 612, 0.5f);
                foreach (var coll in collisions)
                {
                    if (coll.CompareTag(CommonNames.TILE_TAG))
                    {
                        var t = coll.gameObject.GetComponent<Tile>();
                        Debug.Log("-r " + gameObject.name + " from " + t.name);
                        realPos = t.cen - Vector2.right;
                        break;
                    }
                }
            }
            else if (Physics.CheckSphere(tp - Vector3.right * 612, 0.5f))
            {
                //Debug.Log("+r " + gameObject.name + " from " + Physics.OverlapSphere(tp - Vector3.right * 612, 0.5f)[0].name);
                //realPos = Physics.OverlapSphere(tp - Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen + Vector2.right;
                var collisions = Physics.OverlapSphere(tp - Vector3.right * 612, 0.5f);
                foreach (var coll in collisions)
                {
                    if (coll.CompareTag(CommonNames.TILE_TAG))
                    {
                        var t = coll.gameObject.GetComponent<Tile>();
                        Debug.Log("+r " + gameObject.name + " from " + t.name);
                        realPos = t.cen + Vector2.right;
                        break;
                    }
                }
            }
            else if (Physics.CheckSphere(tp + Vector3.forward * 612, 0.5f))
            {
                //Debug.Log("+u " + gameObject.name + " from " + Physics.OverlapSphere(tp + Vector3.forward * 612, 0.5f)[0].name);
                //realPos = Physics.OverlapSphere(tp + Vector3.forward * 612, 0.5f)[0].GetComponent<Tile>().cen + Vector2.up;
                var collisions = Physics.OverlapSphere(tp + Vector3.forward * 612, 0.5f);
                foreach (var coll in collisions)
                {
                    if (coll.CompareTag(CommonNames.TILE_TAG))
                    {
                        var t = coll.gameObject.GetComponent<Tile>();
                        Debug.Log("+u " + gameObject.name + " from " + t.name);
                        realPos = t.cen + Vector2.up;
                        break;
                    }
                }
            }
            else if (Physics.CheckSphere(tp - Vector3.forward * 612, 0.5f))
            {
                //Debug.Log("-u " + gameObject.name + " from " + Physics.OverlapSphere(tp - Vector3.forward * 612, 0.5f)[0].name);
                //realPos = Physics.OverlapSphere(tp - Vector3.forward * 612, 0.5f)[0].GetComponent<Tile>().cen - Vector2.up;
                var collisions = Physics.OverlapSphere(tp - Vector3.forward * 612, 0.5f);
                foreach (var coll in collisions)
                {
                    if (coll.CompareTag(CommonNames.TILE_TAG))
                    {
                        var t = coll.gameObject.GetComponent<Tile>();
                        Debug.Log("-u  " + gameObject.name + " from " + t.name);
                        realPos = t.cen - Vector2.up;
                        break;
                    }
                }
            }
            //diagonals
            //honestly these should never really be called except for the beginning
            //also may be somewhat broken atm
            else if (Physics.CheckSphere(tp - Vector3.forward * 612 - Vector3.right * 612, 0.5f))
            {
                //Debug.Log("--d " + gameObject.name + " from " + Physics.OverlapSphere(tp - Vector3.forward * 612 - Vector3.right * 612, 0.5f)[0].name);
                //realPos = Physics.OverlapSphere(tp - Vector3.forward * 612 - Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen - Vector2.up - Vector2.right;
                var collisions = Physics.OverlapSphere(tp - Vector3.forward * 612 - Vector3.right * 612, 0.5f);
                foreach (var coll in collisions)
                {
                    if (coll.CompareTag(CommonNames.TILE_TAG))
                    {
                        var t = coll.gameObject.GetComponent<Tile>();
                        Debug.Log("--d " + gameObject.name + " from " + t.name);
                        realPos = t.cen - Vector2.up - Vector2.right;
                        break;
                    }
                }

            }
            else if (Physics.CheckSphere(tp - Vector3.forward * 612 + Vector3.right * 612, 0.5f))
            {
                //Debug.Log("-+d " + gameObject.name + " from " + Physics.OverlapSphere(tp - Vector3.forward * 612 + Vector3.right * 612, 0.5f)[0].name);
                //realPos = Physics.OverlapSphere(tp - Vector3.forward * 612 + Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen - Vector2.up + Vector2.right;
                var collisions = Physics.OverlapSphere(tp - Vector3.forward * 612 + Vector3.right * 612, 0.5f);
                foreach (var coll in collisions)
                {
                    if (coll.CompareTag(CommonNames.TILE_TAG))
                    {
                        var t = coll.gameObject.GetComponent<Tile>();
                        Debug.Log("-+d " + gameObject.name + " from " + t.name);
                        realPos = t.cen - Vector2.up + Vector2.right;
                        break;
                    }
                }
            }
            else if (Physics.CheckSphere(tp + Vector3.forward * 612 - Vector3.right * 612, 0.5f))
            {
                //Debug.Log("+-d " + gameObject.name + " from " + Physics.OverlapSphere(tp + Vector3.forward * 612 - Vector3.right * 612, 0.5f)[0].name);
                //realPos = Physics.OverlapSphere(tp + Vector3.forward * 612 - Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen + Vector2.up - Vector2.right;
                var collisions = Physics.OverlapSphere(tp + Vector3.forward * 612 - Vector3.right * 612, 0.5f);
                foreach (var coll in collisions)
                {
                    if (coll.CompareTag(CommonNames.TILE_TAG))
                    {
                        var t = coll.gameObject.GetComponent<Tile>();
                        Debug.Log("+-d " + gameObject.name + " from " + t.name);
                        realPos = t.cen + Vector2.up - Vector2.right;
                        break;
                    }
                }

            }
            else if (Physics.CheckSphere(tp + Vector3.forward * 612 + Vector3.right * 612, 0.5f))
            {
                //Debug.Log("++d " + gameObject.name + " from " + Physics.OverlapSphere(tp + Vector3.forward * 612 + Vector3.right * 612, 0.5f)[0].name);
                //realPos = Physics.OverlapSphere(tp + Vector3.forward * 612 + Vector3.right * 612, 0.5f)[0].GetComponent<Tile>().cen + Vector2.up - Vector2.right;
                var collisions = Physics.OverlapSphere(tp + Vector3.forward * 612 + Vector3.right * 612, 0.5f);
                foreach (var coll in collisions)
                {
                    if (coll.CompareTag(CommonNames.TILE_TAG))
                    {
                        var t = coll.gameObject.GetComponent<Tile>();
                        Debug.Log("++d " + gameObject.name + " from " + t.name);
                        realPos = t.cen + Vector2.up - Vector2.right;
                        break;
                    }
                }
            }

            cen = realPos;

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
            MINUS_R,
            PLUS_R,
            MINUS_U,
            PLUS_U,
            MINUS_MINUS_D,
            MINUS_PLUS_D,
            PLUS_MINUS_D,
            PLUS_PLUS_D
        }

        /*
         * Check if a tile already exists nearby centerPosition given in input. 
         * If yes, updates the center of the tile.
         * **/
        public static Vector2 CheckNearbyTile(Vector3 _tilePosition, 
            Vector2 _realPos,
            string _tileName)
        {

            var result = CheckSphereByOrientation(_tilePosition);

            if (result.IsCollided)
            {
                _realPos = ApplyRecalculation(result.PositionCollision, 
                    0.5f, 
                    _realPos,
                    result.TileOrientation,
                    _tileName, true);
            }

            return _realPos;
        }

        private struct CheckSphereByOrientationResult
        {

            public CheckSphereByOrientationResult(TileOrientationTable _tileOrientation,
             Vector3 _positionCollision,
             bool _isCollided)
            {
                TileOrientation = _tileOrientation;
                PositionCollision = _positionCollision;
                IsCollided = _isCollided;
            }

            public void CalculatePositionToCheckByOrientation(TileOrientationTable _tileOrientation)
            {
                switch (_tileOrientation)
                {
                    case TileOrientationTable.MINUS_R:
                        PositionCollision = PositionCollision - Vector3.right;
                        break;
                    case TileOrientationTable.PLUS_R:
                        PositionCollision = PositionCollision + Vector3.right;
                        break;
                    case TileOrientationTable.MINUS_U:
                        PositionCollision = PositionCollision - Vector3.up;
                        break;
                    case TileOrientationTable.PLUS_U:
                        PositionCollision = PositionCollision + Vector3.up;
                        break;
                    case TileOrientationTable.MINUS_MINUS_D:
                        PositionCollision = PositionCollision - Vector3.right - Vector3.up;
                        break;
                    case TileOrientationTable.PLUS_PLUS_D:
                        PositionCollision = PositionCollision + Vector3.right + Vector3.up;
                        break;                              
                    case TileOrientationTable.MINUS_PLUS_D:
                        PositionCollision = PositionCollision - Vector3.right + Vector3.up;
                        break;                              
                    case TileOrientationTable.PLUS_MINUS_D:
                        PositionCollision = PositionCollision + Vector3.right - Vector3.up;
                        break;
                }
            }

            public TileOrientationTable TileOrientation;
            public Vector3 PositionCollision;
            public bool IsCollided;
        }

        private static CheckSphereByOrientationResult CheckSphereByOrientation(Vector3 _tilePosition)
        {
            CheckSphereByOrientationResult result = 
                new CheckSphereByOrientationResult(TileOrientationTable.MINUS_R,_tilePosition,false);

            foreach (var orientation in Enum.GetValues(typeof(TileOrientationTable))) {
                result.CalculatePositionToCheckByOrientation((TileOrientationTable)orientation);
                if (Physics.CheckSphere(result.PositionCollision, 0.5f))
                {
                    result.IsCollided = true;
                    break;
                }
            }

            return result;
        }

        /*
         * Recalculate position only if CheckNearbyTile founded a tile.
         * **/
        private static Vector2 ApplyRecalculation(Vector3 _centerCheckSphere,
                    float _radiusCheckSphere,
                    Vector2 _realPos,
                    TileOrientationTable _tileOrientation,
                    string tileName,
                    bool _activateLog = false)

        {
            //Checks only for Tiles
            var allOverlaps = Physics.OverlapSphere(_centerCheckSphere, _radiusCheckSphere);
            var tileCollider = allOverlaps.FirstOrDefault(x => {
                return x.CompareTag(CommonNames.TILE_TAG);
            });

            if (!ReferenceEquals(tileCollider, null))
            {
                var tile = tileCollider.gameObject.GetComponent<Tile>();
                if(_activateLog)
                    Debug.Log(_tileOrientation.ToString() + " "+tileName + " from " + tile.name);

                _realPos = RecalculatePositionByOrientation(_realPos, tile.cen ,_tileOrientation);
            }
            return _realPos;
        }

        private static Vector2 RecalculatePositionByOrientation(
            Vector2 _pos,
            Vector2 _cen,
            TileOrientationTable _tileOrientation) {
            switch (_tileOrientation) {
                case TileOrientationTable.MINUS_R:
                    _pos = _cen - Vector2.right;
                    break;
                case TileOrientationTable.PLUS_R:
                    _pos = _cen + Vector2.right;
                    break;
                case TileOrientationTable.MINUS_U:
                    _pos = _cen - Vector2.up;
                    break;
                case TileOrientationTable.PLUS_U:
                    _pos = _cen + Vector2.up;
                    break;
                case TileOrientationTable.MINUS_MINUS_D:
                    _pos = _cen - Vector2.right - Vector2.up;
                    break;
                case TileOrientationTable.PLUS_PLUS_D:
                    _pos = _cen + Vector2.right + Vector2.up;
                    break;
                case TileOrientationTable.MINUS_PLUS_D:
                    _pos = _cen - Vector2.right + Vector2.up;
                    break;
                case TileOrientationTable.PLUS_MINUS_D:
                    _pos = _cen + Vector2.right - Vector2.up;
                    break;
            }
            return _pos;
        }

    }
}
