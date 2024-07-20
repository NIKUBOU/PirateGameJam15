using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Coloring")]
    [SerializeField] private Material baseColor, offsetColor;

    public void Init(bool isOffset)
    {
        //Fetches the meshRenderer of the tile to edit it's color
        MeshRenderer mR = GetComponent<MeshRenderer>();

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
