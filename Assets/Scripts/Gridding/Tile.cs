using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Coloring")]
    [SerializeField] private Material baseColor;
    [SerializeField] private Material offsetColor;
    [SerializeField] private Material cityBaseColor;
    [SerializeField] private Material cityOffsetColor;

    public void Init(bool isOffset, bool isCity)
    {
        //Fetches the meshRenderer of the tile to edit it's color
        MeshRenderer mR = GetComponent<MeshRenderer>();

        //Checks if it's a city
        if (isCity)
        {
            //Changes the colors to the city variants before applying them
            baseColor = cityBaseColor;
            offsetColor = cityOffsetColor;
        }

        //Applies the colors
        if (isOffset)
        {
            mR.material = offsetColor;
        }
        else
        {
            mR.material = baseColor;
        }
    }
}
