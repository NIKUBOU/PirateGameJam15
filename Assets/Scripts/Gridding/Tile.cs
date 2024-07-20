using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material baseColor, offsetColor;

    public void Init(bool isOffset)
    {
        MeshRenderer mR = GetComponent<MeshRenderer>();

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
