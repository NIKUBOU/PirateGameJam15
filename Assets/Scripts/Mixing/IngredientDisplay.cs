using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientDisplay : MonoBehaviour
{
    [SerializeField] private ScriptableIngredient scriptableIngredient;

    private void Awake()
    {
        //Feeds all the stuff to the ingredient display
        GetComponent<Image>().sprite = scriptableIngredient.GetSprite();
    }

    public ScriptableIngredient GetIngredient()
    {
        return scriptableIngredient;
    }
}
