using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientDisplay : MonoBehaviour
{
    [SerializeField] private Ingredient ingredient;
    public Ingredient Ingredient {  get { return ingredient; } }

    private void Awake()
    {
        //Feeds all the stuff to the ingredient display
        GetComponent<Image>().sprite = ingredient.Logo;
    }
}
