using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Mixing/Recipe")]
public class Recipe : ScriptableObject
{
    [Tooltip("Put the ingredients required to make the item here")]
    [SerializeField] private Ingredient[] ingredients;
    public Ingredient[] Ingredients {  get { return ingredients; } }

    [SerializeField] private Gas resultGas;
    public Gas ResultGas { get { return resultGas; } }
}
