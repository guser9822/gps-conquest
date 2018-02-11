using UnityEngine;
using System.Collections;
using System.Linq;
using Assets;
using TC.GPConquest.Player;

/// <summary>
/// Breve introduzione : Il seguente progetto è in sostanza un clone di Pokemon-Go / Ingress con diverse aggiunte e personalizzazioni.
/// Come funziona in breve ? 
/// I moderni sistemi di posizionamento globale (GPS) utilizzano un sistema di coordinate geografiche geodetico chiamato WGS84 (o EPSG:4326)
/// per fornirci latitudine e longitudine (espresse in gradi) della nostra attuale posizione. In tale sistema di riferimento la terra
/// viene approssimata ad un ellissoide con particolari caratteristiche. Per realizzare una mappa in Unity su cui far spostare il nostro avator
/// secondo le coordinate geografiche correnti, è necessario effettuare quindi una proiezione di tali coordinate (basate su di un ellisse) su di una 
/// mappa in due dimensioni (x e z nel particolare caso di Unity). Per fare ciò si utilizzano le proiezioni Web Mercator(o EPSG.900913 o EPSG:3857), 
/// basate sulle proiezioni Mercator e ideate da Google, che vengono utilizzate da tutti i servizi di street maps online (BingMap,Open Street Map,
/// GoogleMap etc) che forniscono una mappa del mondo in due dimensioni, navigabile e con diversi livelli di dettaglio (zoom);  tali proiezioni si 
/// basano su un modello sferico della terra piuttosto che ellissoidale per semplificare i calcoli e consistono sostanzialmente in una rappresentazione 
/// del mondo su di una cartina geografica piatta, suddivisa in tiles (tasselli) a seconda del livello di zoom. La funzione calcTile calcola il tile
/// iniziale in cui il giocatore è geograficamente localizzato (vedere la funzione per ulteriori informazioni).
/// Una volta ottenute le coordinate in pixel  delle proprie coordinate geografiche all'interno del tile, queste vengono utilizzate nell'API Vector Tile
/// di MapZen per ricavare un file di tipo JSON (del tile in cui è geolocalizzato il giocatore) contentente una descrizione matematica e metadati di
/// palazzi,strade e altre caratteristiche del layer di base di OpenStreetMap(vedere la classe Tile per ulteriori informazioni).
/// </summary>
public class tileGen : MonoBehaviour
{
    Vector2[,] localTerrain = new Vector2[3, 3];
    GameObject[,] tiles = new GameObject[3, 3];
    float currX;
    float currZ;
    float oldX;
    float oldZ;
    public GameObject tile;

    public Vector2 LatLng; 
    public Vector2 Center;
    public Vector2 startTile;
    private Vector2 Position;
    private int Zoom = 16;
    string status = "start";
    public bool editorMode;

    protected DestinationController DestinationController;
    protected Vector3 CalcPlayerPosition;
    public bool isReady;

    private void Awake()
    {
        DestinationController = GetComponent<DestinationController>();
    }

    // Use this for initialization
    // Punto di partenza del gioco. La funzione Start() può essere una coroutine.
    public IEnumerator StartTiling()
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        SimplePool.Preload(tile, 15);
        currX = oldX = Mathf.Floor(transform.position.x);
        currZ = oldZ = Mathf.Floor(transform.position.z);
        // First, check if user has location service enabled
        // No location then fallback coordinates
        if (!Input.location.isEnabledByUser||editorMode)
        {
            //CDN 40.857133f, 14.278698f
            //Bologna 44.490346 11.327496
            float lat = 40.857133f;
            float longi = 14.278698f;
            
            LatLng = new Vector2(lat, longi);
            Center = calcTile(lat, longi);
            Debug.Log("Center: " + Center);
            startTile = Center;
            Position = posInTile(lat, longi);
            
            Debug.Log(Position);
            //Posiziona il giocatore rispetto al centro del tile. La grandeza del tile in pixel è 256, ovvero 611 unità in Unity
            CalcPlayerPosition = new Vector3((Position.x - 0.5f) * 611, 0, (0.5f - Position.y) * 611);
            /*
             * Move the destination controller on the network
             * **/
            DestinationController.MovePlayerDestination(CalcPlayerPosition);
            Debug.Log("Calculated player position in the tile :"+CalcPlayerPosition);
            status = "no location service";

            //Il terreno di gioco attuale è composto da 9 tile. Il Tile centrale è quello iniziale, dato dalle coordinate geografiche
            //del giocatore ottenute all'avvio del gioco
            tiles[0, 0] = SimplePool.Spawn(tile, Vector3.zero, Quaternion.identity);
            //Coroutine che andrà a popolare direttamente il tile iniziale con le proprie costruzioni,strade,acque e parchi
            StartCoroutine(tiles[0, 0].GetComponent<Assets.Tile>().CreateTile(new Vector2(Center.x, Center.y), Vector3.zero, 16));

            updateBoard();
            //Enable player UI after the scene is loaded

            isReady = true;

            yield break;
        }

        //THE REST OF THIS CODE NEED TO BE FIXED, IT DOESN'T UPDATES THE PLAYER POSITION THROUGH GPS COORDINATES

        // Start service before querying location
        //Input.location.Start(5,5f);
        //status = "rev up";

        //// Wait until service initializes
        //int maxWait = 20;
        //while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        //{
        //    yield return new WaitForSeconds(1);
        //    maxWait--;
        //}

        //// Service didn't initialize in 20 seconds
        //if (maxWait < 1)
        //{
        //    status = "timed out";
        //    print("Timed out");
        //    yield break;
        //}

        //// Connection has failed
        //if (Input.location.status == LocationServiceStatus.Failed)
        //{
        //    status = "Unable to determine device location";
        //    print("Unable to determine device location");
        //    yield break;
        //}
        //else
        //{
        //    // Access granted and location value could be retrieved
        //    print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        //    LatLng = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        //    Center = calcTile(Input.location.lastData.latitude, Input.location.lastData.longitude);
        //    Debug.Log(Center);
        //    startTile = Center;
        //    Position = posInTile(Input.location.lastData.latitude, Input.location.lastData.longitude);
        //    Debug.Log(Position);
        //    status = "Creating tile " + Center.x + ", " + Center.y;
        //    status = "Pos tile " + Position.x + ", " + Position.y;
        //    Vector3 pos = new Vector3((Position.x - 0.5f) * 611, 0, (0.5f - Position.y) * 611);
        //    /*
        //     * Move the destination controller on the network
        //     * **/
        //    //DestinationController.MovePlayerDestination(pos);

        //    tiles[0,0] = SimplePool.Spawn(tile, Vector3.zero, Quaternion.identity);
        //    StartCoroutine(tiles[0, 0].GetComponent<Assets.Tile>().CreateTile(new Vector2(Center.x, Center.y), Vector3.zero, 16));

        //    updateBoard();
        //    InvokeRepeating("updateLoc", 2f, 2f);
        //}
    }

    void updateLoc()
    {
        //updates location
        status = "repeating";
        LatLng = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        Center = calcTile(Input.location.lastData.latitude, Input.location.lastData.longitude);
        Debug.Log(Center);
        Position = posInTile(Input.location.lastData.latitude, Input.location.lastData.longitude);
        Debug.Log(Position);
        Vector3 pos = new Vector3((Position.x - 0.5f) * 611, 0, (0.5f - Position.y) * 611);
        ///*
        // * Move the destination controller on the network
        // * **/
        //DestinationController.MovePlayerDestination(pos);
    }

    // checks if movement is greate than a single tile space, if so update the board
    void Update()
    {
        if (isReady)
        {
            currX = Mathf.Floor(transform.position.x) - Mathf.Floor(transform.position.x) % 612;
            currZ = Mathf.Floor(transform.position.z) - Mathf.Floor(transform.position.z) % 612;
            if (Mathf.Abs(currX - oldX) > 306 || Mathf.Abs(currZ - oldZ) > 306)
            {
                Debug.Log("UPDATE BOARD");
                updateBoard();
                oldX = currX;
                oldZ = currZ;
            }
        }
     }

    //checks if theres a tile in that location, if not then put one down
    void updateBoard()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                localTerrain[i + 1, j + 1] = new Vector2(Center.x + i, Center.y + j);

                if (!Physics.CheckSphere(new Vector3(currX + i * 612, 0, currZ + j * 612), 0.4f))
                {
                    tiles[i + 1, j + 1] = SimplePool.Spawn(tile, new Vector3(currX + i * 612, 0f, currZ + j * 612), Quaternion.identity);
                    StartCoroutine(tiles[i + 1, j + 1].GetComponent<Tile>().CreateTile(new Vector2(Center.x + i, Center.y - j), 
                        new Vector3(currX + i * 612, 0f, currZ + j * 611), 
                        16));
                }
                else {
                    Collider[] temp = Physics.OverlapSphere(new Vector3(currX + i * 612, 0f, currZ + j * 612), 0.4f);
                    tiles[i + 1, j + 1] = temp[0].gameObject;
                }
            }
        }
        cleanup(0);
    }

    //cleanup tiles outside a certain range
    void cleanup(float sec)
    {
        Collider[] include = Physics.OverlapSphere(transform.position, 1800f);
        Collider[] exclude = Physics.OverlapSphere(transform.position, 10000f);
        var outside = exclude.Except(include);
        foreach (Collider g in outside)
        {
            if (g.tag == "Tile")
            {
                g.GetComponent<Tile>().Cleanup();
                SimplePool.Despawn(g.gameObject);
            }
        }
    }

    /// <summary>
    /// Date le coordinate geografiche lat/long, calcola il tile in EPSG:900913
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="lng"></param>
    /// <returns></returns>
    Vector2 calcTile(float lat, float lng)
    {
        //Fonti :
        //https://mapzen.com/projects/vector-tiles/                <- Piattaforma tiles utilizzata da questo progetto
        //http://wiki.openstreetmap.org/wiki/Slippy_map_tilenames  <- Convenzioni e nomi del servizio di tiles utilizzato da questo progetto

        //Introduzione:
        //In questo progetto, tramite l'API Vector Tile di MapZen, utilizza come servizio di tiling quello di Open  Street Map.
        //OSM è un database geografico mondiale pubblico che contiene enormi quantità di informazioni relative a strutture, strade, parchi
        //etc. Per rendere disponibili queste enormi quantità di dati come mappe in due dimensioni consultabili online, queste vengono
        //frammentate come immagini uniformi 256x256 pixel chiamate tile. A seconda della risoluzione scelta, un insieme di tile coprirà
        //tutta la superficie della terra e dunque date le coordinate geografiche lat/long, questo algoritmo le converte in coordinate pixel x/y
        //di determinato tile.
        //
        //n = 2 ^(2* zoom)  Calcola il numero di tiles necessari per mostrare tutto il mondo con tale zoom (livello di dettaglio)
        //
        //Calcola le coordinate in pixel x/y all'interno del tile rispetto le coordinate geografiche lat/long
        //xtile = n * ((lon_deg + 180) / 360)
        //ytile = n * (1 - (log(tan(lat_rad) + sec(lat_rad)) / π)) / 2

        float n = Mathf.Pow(2, Zoom);
        float xtile = n * ((lng + 180) / 360);
        float ytile = n * (1 - (Mathf.Log(Mathf.Tan(Mathf.Deg2Rad * lat) + 
                            (1f / Mathf.Cos(Mathf.Deg2Rad * lat))) / Mathf.PI)) / 2f;
        //Le coordinate in pixel sono espresse tramite interi
        return new Vector2((int)xtile, (int)ytile);
    }

    /// <summary>
    /// Calcola la posizine del giocatore rispetto al centro del tile in cui esso è geograficamente localizzato
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="lng"></param>
    /// <returns></returns>
    Vector2 posInTile(float lat, float lng)
    {
        float n = Mathf.Pow(2, Zoom);
        float xtile = n * ((lng + 180) / 360);
        float ytile = n * (1 - (Mathf.Log(Mathf.Tan(Mathf.Deg2Rad * lat) + (1f / Mathf.Cos(Mathf.Deg2Rad * lat))) / Mathf.PI)) / 2f;
        return new Vector2(xtile - (int)xtile, ytile - (int)ytile);
    }

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 70, 500, 30),debugString);
    //}
}
