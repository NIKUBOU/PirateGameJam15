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
    [SerializeField] private MixingSlot[] mixingSlots;

    private CursorTools cT;

    //Variables
    private ScriptableIngredient currentIngredient;

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
            if (currentIngredient != null)
            {
                //Disable the image on the cursor
                cT.DisableCursorImage();

                //Setup for dropping off the item
                MixingSlot nearestMixingSlot = null;
                float shortestDistance = float.MaxValue;

                //Check which slot is the closest to the mouse cursor
                foreach (MixingSlot mixingSlot in mixingSlots)
                {
                    float dist = Vector2.Distance(Input.mousePosition, mixingSlot.transform.position);

                    //Friendly little contest between the slots lol
                    if (dist < shortestDistance)
                    {
                        shortestDistance = dist;
                        nearestMixingSlot = mixingSlot;
                    }
                }

                //Make sure the slot is active
                nearestMixingSlot.gameObject.SetActive(true);

                //Put the image of the item in the slot
                nearestMixingSlot.GetComponent<Image>().sprite = currentIngredient.GetSprite();

                //Store the item properties in the slot
                nearestMixingSlot.StoredItem = currentIngredient;

                //Drop the item
                currentIngredient = null;
            }
        }
    }

    //Manages the dropping of items into a slot
    public void DropItemInSlot(IngredientDisplay ingredientDisplay)
    {
        if (currentIngredient == null)
        {
            currentIngredient = ingredientDisplay.GetIngredient();
            cT.ChangeActiveCursorImage(currentIngredient.GetSprite());
        }
    }
}
