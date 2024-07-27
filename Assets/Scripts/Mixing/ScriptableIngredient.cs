using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Mixing/Ingredient")]
public class ScriptableIngredient : ScriptableObject
{
    [SerializeField] private Sprite logo;

    public Sprite GetSprite()
    {
        return logo;
    }
}
