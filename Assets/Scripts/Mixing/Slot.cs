using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    //Can be fed an item and it will keep it
    private Item storedItem;
    public Item StoredItem { get { return storedItem; } set { storedItem = value; } }
}
