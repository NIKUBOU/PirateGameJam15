using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class GridManager : MonoBehaviour
{
    /// <summary>
    /// https://www.youtube.com/watch?v=kkAjpQAM-jE
    /// </summary>
    /// 

    //GridManager Instance
    private static GridManager instance;

    public static GridManager Instance { get { return instance; } }

    [Header("Grid Layout")]

    //Controls the width of the grid
    [Tooltip("Keep your aspect ratio in mind, but remove a few tiles for the mixing UI (by default, keep a 16:9 ratio)")]
    [SerializeField] private int gridWidth;

    //Controls the height of the grid
    [Tooltip("Keep your aspect ratio in mind, must be an even number (by default, keep a 16:9 ratio)")]
    [SerializeField] private int gridHeight;

    //Holds the tile prefab so that the code can spawn it
    [SerializeField] private Tile tilePrefab;



    [Header("City management")]

    //Controls how many cities can spawn at the start of the game
    [SerializeField] private int maxCitiesAtStart;

    //Controls the rng of the city spawning, a high value will result in more spawns
    [Tooltip("The higher the value, the more spaced out the cities will be (be warned that this may spawn lower the amount of cities that spawn)")]
    [SerializeField] private int cityRandomizationFactor;

    //Holds all the cities currently ingame
    private List<Tile> currentCities;

    //Controls how often do cities expand
    [Tooltip("A higher value will cause expansion cycles to have more time between them")]
    [SerializeField] private float timeBetweenCityExpansionCycles;

    //How many % cities grow every tick
    [Tooltip("Controls the % of how many cities grow every tick (values can only range from 0 to 100")]
    [SerializeField] private int cityGrowthPourcentage;

    //At which point do cities stop tiering up
    [SerializeField] private int cityMaxTier;

    //What is the required tier for cities to start growing
    [Tooltip("At what city tier do they start expanding?")]
    [SerializeField] private int cityTierRequiredToGrow;



    [Header("Plane Management")]

    //Holds the plane spawner prefab
    [SerializeField] private PlaneSpawner planeSpawner;
    
    //Holds all the plane spawners currently ingame
    private List<PlaneSpawner> planeSpawners;

    //Controls how often do cities expand
    [Tooltip("A higher value will cause plane spawns to have more time between them")]
    [SerializeField] private float timeBetweenPlaneSpawns;




    [Header("Camera control")]

    //Just checks if you want the camera to be moved or not
    [Tooltip("Controls wether the camera automatically calibrates itself to the grid")]
    [SerializeField] private bool centerCamToGrid;

    //Holds the camera's position so it can be moved at the start of the game
    [Tooltip("Unnecessary if you don't want to center the camera")]
    [SerializeField] private Camera cam;

    private void Awake()
    {
        CreateInstance();
    }

    //Creates an instance of this manager
    private void CreateInstance()
    {
        //Instance stuff
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        //Fix values
        cityGrowthPourcentage = Mathf.Clamp(cityGrowthPourcentage, 0, 100);

        //Move the camera
        MoveCamera();

        //Generates the grid
        GenerateGrid();

        //Starts the growth cycle
        StartCoroutine(CallDevelopCities());

        //Starts the plane spawning cycle
        StartCoroutine(CallPlaneSpawn());
    }

    //Moves the camera to the center of the grid if needed
    private void MoveCamera()
    {
        //Tweaks fov
        cam.orthographicSize = gridHeight / 2;
        float cameraWidth = gridHeight * cam.aspect;

        // Adjust the camera's position
        // Align the bottom left of the camera to the bottom left of the grid
        cam.gameObject.transform.position = new Vector3(-.5f + cameraWidth / 2, gridHeight, -.5f + gridHeight / 2);

        // Set the camera to look straight down at the grid
        cam.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void GenerateGrid()
    {
        //Creates a list to store the current cities
        currentCities = new List<Tile>();

        //Takes in the height and width values to generated a grid with that many rows and columns
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                //Spawns a grid tile
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {z}";

                //This part is just to cycle between the base and the offset colors to get a grid pattern (can be removed as needed)
                var isOffset = (x % 2 == 0 && z % 2 != 0) || (x % 2 != 0 && z % 2 == 0);

                //Generates cities
                var isCity = false;

                if (maxCitiesAtStart > currentCities.Count && Random.Range(0, cityRandomizationFactor) == 0)
                {
                    isCity = true;
                    currentCities.Add(spawnedTile);
                }

                //Failsafe spawn 1 city if no cities generated by the time we reach the last tile
                if (x == gridWidth - 1 && z == gridHeight - 1 && currentCities.Count < maxCitiesAtStart)
                {
                    isCity = true;
                    currentCities.Add(spawnedTile);
                }

                //Spawns the tile objects
                spawnedTile.Init(isOffset, isCity, x, z);
            }
        }

        //Generates the plane spawners
        GenerateSpawners();
    }

    private void GenerateSpawners()
    {
        //Create a list to store the planeSpawners
        planeSpawners = new List<PlaneSpawner>();


        // Iterate over the grid width and height to place plane spawners on the borders
        for (int x = -1; x <= gridWidth; x++)
        {
            for (int z = -1; z <= gridHeight; z++)
            {
                // Skip corners
                if ((x == -1 || x == gridWidth) && (z == -1 || z == gridHeight))
                {
                    continue;
                }

                // Place spawners along the border
                if (x == -1 || x == gridWidth || z == -1 || z == gridHeight)
                {
                    //Spawns the spawner
                    var spawner = Instantiate(planeSpawner, new Vector3(x, .5f, z), Quaternion.identity);
                    //Changes it's name so it's easier to find
                    spawner.name = $"Spawner {x} {z}";
                    //Adds it to the list of spawners so it can be randomly selected later
                    planeSpawners.Add(spawner);

                    // Determine the direction to face based on the sphere's position
                    Vector3 directionToFace = x switch
                    {
                        -1 => Vector3.right, // Left border, face right
                        _ when x == gridWidth => Vector3.left, // Right border, face left
                        _ when z == -1 => Vector3.forward, // Bottom border, face up
                        _ => Vector3.back // Top border, face down
                    };

                    // Set the sphere's rotation to face the determined direction
                    spawner.transform.rotation = Quaternion.LookRotation(directionToFace);
                }
            }
        }
    }

    //Does a growth cycle after x amount of time
    private IEnumerator CallDevelopCities()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenCityExpansionCycles);
            DevelopCities();
        }
    }

    //Does a spawn cycle after x amount of time
    private IEnumerator CallPlaneSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenPlaneSpawns);
            PlaneSpawn();
        }
    }

    //Makes the cities grow by one tile
    private void DevelopCities()
    {
        //Creates a list of tiles to modify (otherwise when a new city grows it would consider the new city as ungrown)
        List<Tile> citiesToGrow = new List<Tile>();

        //Checks if the city is upgradable (either by tier up or by colonization)
        foreach (Tile city in currentCities)
        {
            if (city.TileTier < cityMaxTier || FindColonizableNeighbors(city).Count > 0)
            {
                citiesToGrow.Add(city);
            }
        }

        int citiesToGrowCount = Mathf.CeilToInt(citiesToGrow.Count * cityGrowthPourcentage / 100f);
        //Grows a random city
        for (int gc = 0; gc < citiesToGrowCount; gc++)
        {
            //Picks a random city
            var cityToUpgrade = citiesToGrow[Random.Range(0, citiesToGrow.Count)];

            //Upgrades the random city
            UpgradeCity(cityToUpgrade);
        }
    }

    public void RemoveCityFromList(Tile cityToRemove)
    {
        currentCities.Remove(cityToRemove);
    }

    private void PlaneSpawn()
    {
        //Picks a random spawner
        var spawnerToSpawn = planeSpawners[Random.Range(0, planeSpawners.Count)];

        //Spawns a plane from that spawner
        spawnerToSpawn.SpawnPlane();

    }

    private void UpgradeCity(Tile cityToUpgrade)
    {
        //Create a neighboring city if possible
        if (cityToUpgrade.TileTier >= cityTierRequiredToGrow)
        {
            ColonizeNeighbor(cityToUpgrade);
        }

        //Upgrades the city's tier if possible
        if (cityToUpgrade.TileTier < cityMaxTier)
        {
            cityToUpgrade.TierUp();
        }
    }

    //Checks neighboring tiles if they already are cities
    private List<Tile> FindColonizableNeighbors(Tile city)
    {
        var currentTileXID = city.XID;
        var currentTileZID = city.ZID;

        // Array to store the coordinates of the neighboring tiles
        var neighbors = new (int x, int z)[]
        {
                (currentTileXID - 1, currentTileZID), // left
                (currentTileXID + 1, currentTileZID), // right
                (currentTileXID, currentTileZID + 1), // above
                (currentTileXID, currentTileZID - 1)  // below
        };

        //Just a list of all the neighboring grass tiles so one can be chosen at random to grow the city
        List<Tile> nonCityNeighborTiles = new List<Tile>();

        // Iterate through the neighbors and perform checks
        foreach (var (x, z) in neighbors)
        {
            // Check if the coordinates are within grid bounds
            if (x >= 0 && x < gridWidth && z >= 0 && z < gridHeight)
            {
                var neighborTile = GameObject.Find($"Tile {x} {z}").GetComponent<Tile>();

                if (neighborTile != null && !currentCities.Contains(neighborTile))
                {
                    nonCityNeighborTiles.Add(neighborTile);
                }
            }
        }

        return nonCityNeighborTiles;
    }

    private void ColonizeNeighbor(Tile tileToGrow)
    {
        List<Tile> nonCityNeighborTiles = FindColonizableNeighbors(tileToGrow);

        //Pick a random neighbor
        if (nonCityNeighborTiles.Count > 0)
        {
            var randomIndex = Random.Range(0, nonCityNeighborTiles.Count);
            var randomNonCityTile = nonCityNeighborTiles[randomIndex];

            //Turns the selected random tile into a city
            randomNonCityTile.Colonize();
            currentCities.Add(randomNonCityTile);
        }
    }
}


