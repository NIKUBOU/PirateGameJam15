using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gas", menuName = "Mixing/Gas")]
public class Gas : ScriptableObject
{
    [SerializeField] private Sprite logo;

    public Sprite GetSprite()
    {
        return logo;
    }
}
