using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixingManager : MonoBehaviour
{
    /// <summary>
    /// https://www.youtube.com/watch?v=1fbd-yTcMgY&t=403s
    /// </summary>

    //MixingManager Instance
    private static MixingManager instance;

    public static MixingManager Instance { get { return instance; } }

    //Setups
    [SerializeField] private Canvas uI;
    [SerializeField] private Slot[] mixingSlots;
    [Tooltip("Here you can create your recipes")]
    [SerializeField] private List<Item> gazList;
    private CursorTools cT;

    //Variables
    private Item currentItem;

    private void Awake()
    {
        CreateInstance();
    }

    private void Start()
    {
        GetUI();
    }

    private void Update()
    {
        CheckIfItemIsGettingDropped();
    }

    //Creates an instance of this manager
    private void CreateInstance()
    {
        //Instance stuff
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    //Grabs the items it needs from the ui
    private void GetUI()
    {
        //Finds the CursorTools script
        cT = FindObjectOfType<CursorTools>();
    }

    private void CheckIfItemIsGettingDropped()
    {
        //When let go...
        if (Input.GetMouseButtonUp(0))
        {
            if (currentItem != null)
            {
                //Disable the image on the cursor
                cT.DisableCursorImage();

                //Setup for dropping off the item
                Slot nearestSlot = null;
                float shortestDistance = float.MaxValue;

                //Check which slot is the closest to the mouse cursor
                foreach (Slot slot in mixingSlots)
                {
                    float dist = Vector2.Distance(Input.mousePosition, slot.transform.position);

                    //Friendly little contest between the slots lol
                    if (dist < shortestDistance)
                    {
                        shortestDistance = dist;
                        nearestSlot = slot;
                    }
                }

                //Make sure the slot is active
                nearestSlot.gameObject.SetActive(true);

                //Put the image of the item in the slot
                nearestSlot.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;

                //Store the item properties in the slot
                nearestSlot.StoredItem = currentItem;

                //Drop the item
                currentItem = null;
            }
        }
    }

    //Manages the dropping of items into a slot
    public void DropItemInSlot(Item item)
    {
        if (currentItem == null)
        {
            currentItem = item;
            cT.ChangeActiveCursorImage(item.GetComponent<Image>());
        }
    }
}
