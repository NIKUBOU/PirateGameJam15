using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Layout")]

    [Tooltip("Keep your aspect ratio in mind (go with multiples of 16)")]
    [SerializeField] private int gridWidth;

    [Tooltip("Keep your aspect ratio in mind (go with multiples of 9)")]
    [SerializeField] private int gridHeight;
    [SerializeField] private Tile tilePrefab;

    [Header("Camera control")]
    [SerializeField] private Transform cam;
    [SerializeField] private bool centerCamToGrid;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
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
                spawnedTile.Init(isOffset);
            }
        }

        //Moves the camera to the center of the grid if needed
        if (centerCamToGrid)
        {
            cam.transform.position = new Vector3 ((float)gridWidth/2 - .5f, 10, (float)gridHeight/2 - .5f);
            cam.transform.rotation = Quaternion.Euler(90, 0, 0); 
        }
    }
}
