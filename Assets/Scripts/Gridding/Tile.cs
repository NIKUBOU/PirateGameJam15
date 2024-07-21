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

    private bool offsetTile;

    public void Init(bool isOffset, bool isCity)
    {
        //Fetches the meshRenderer of the tile to edit it's color
        MeshRenderer mR = GetComponent<MeshRenderer>();

        //Applies the colors
        if (isOffset)
        {
            if (isCity)
            {
                mR.material = cityOffsetColor;
            }
            else
            {
                mR.material = offsetColor;
            }
        }
        else
        {
            if (isCity)
            {
                mR.material = cityBaseColor;
            }
            else
            {
                mR.material = baseColor;
            }
        }

        //Retains the offset factor for coloring down the line
        offsetTile = isOffset;
    }
}
