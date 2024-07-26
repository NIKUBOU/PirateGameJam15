using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item stats")]
    
    //Holds the name of the item
    [SerializeField] private string itemName;

    [Tooltip("Put as true if you need to combine two ingredients to get this gaz")]
    [SerializeField] private bool isGaz;
}
