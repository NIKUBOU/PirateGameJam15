using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Coloring")]
    [SerializeField] private Material baseColor;
    [SerializeField] private Material offsetColor;
    [SerializeField] private Material cityBaseColor;
    [SerializeField] private Material cityOffsetColor;
    [SerializeField] private GameObject[] cityTiers;

    private MeshRenderer mR;
    private bool offsetTile;

    private int tileTier = 0;
    public int TileTier { get { return tileTier; } }

    //Stats
    private int xID;
    private int zID;
    public int XID { get { return xID; } }
    public int ZID { get { return zID; } }

    public void Init(bool isOffset, bool isCity, int _xID, int _zID)
    {
        //Fetches the meshRenderer of the tile to edit it's color
        mR = GetComponent<MeshRenderer>();

        //The city tier
        if (isCity)
        {
            TierUp();
        }

        //Retains the offset factor for coloring down the line
        offsetTile = isOffset;

        //Color the tile
        Color();

        //Remembers the tile's ID
        xID = _xID;
        zID = _zID;
    }

    public void Colonize()
    {
        if (offsetTile)
        {
            mR.material = cityOffsetColor;
        }
        else
        {
            mR.material = cityBaseColor;
        }

        TierUp();
    }

    private void Color()
    {
        if (tileTier > 0)
        {
            mR.material = offsetTile ? cityOffsetColor : cityBaseColor;
        }
        else
        {
            mR.material = offsetTile ? offsetColor : baseColor;
        }
    }

    private void RenderBuildings()
    {
        if (tileTier > 0)
        {
            if (tileTier >= 2)
            {
                cityTiers[tileTier - 2].SetActive(false);
            }
            cityTiers[tileTier - 1].SetActive(true);
        }
        else
        {
            foreach (var cityBuilding in cityTiers)
            {
                cityBuilding.SetActive(false);
            }
        }
    }

    //Upgrades the tier of the tile
    public void TierUp()
    {
        tileTier++;
        RenderBuildings();
    }

    public void DestroyCity()
    {
        //Adds score = tile tier
        GameManager.Instance.AddScore(tileTier);

        //Reset tile tier
        tileTier = 0;

        //Recolor it
        Color();

        //Remove it form the list of cities
        GridManager.Instance.RemoveCityFromList(this);

        //Remove buildings
        RenderBuildings();
    }
}
