using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingSlot : MonoBehaviour
{
    //Can be fed an item and it will keep it
    private ScriptableIngredient storedItem;
    public ScriptableIngredient StoredItem { get { return storedItem; } set { storedItem = value; } }
}
