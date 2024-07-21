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
    private MeshRenderer mR;

    private bool offsetTile;
    private int xID;
    private int zID;
    public int XID { get { return xID; } }
    public int ZID { get { return zID; } }

    public void Init(bool isOffset, bool isCity, int _xID, int _zID)
    {
        //Fetches the meshRenderer of the tile to edit it's color
        mR = GetComponent<MeshRenderer>();

        //Applies the colors
        if (isCity)
        {
            mR.material = isOffset ? cityOffsetColor : cityBaseColor;
        }
        else
        {
            mR.material = isOffset ? offsetColor : baseColor;
        }

        //Retains the offset factor for coloring down the line
        offsetTile = isOffset;

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
    }
}
