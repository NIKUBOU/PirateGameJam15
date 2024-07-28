using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
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

    [Tooltip("Place the slots where you drop ingredients to mix")]
    [SerializeField] private MixingSlot[] mixingSlots;

    [Tooltip("Place the slot where the mixed gas will go")]
    [SerializeField] private InventorySlot resultSlot;

    [Tooltip("Place all available recipes")]
    [SerializeField] private Recipe[] recipes;

    [Tooltip("Place all slots used to store gases")]
    [SerializeField] private InventorySlot[] inventorySlots;

    private CursorTools cT;

    //Variables
    private Ingredient currentIngredient;
    private List<Ingredient> currentIngredients;
    private Gas currentGas;
    private InventorySlot currentSlot;

    private void Awake()
    {
        CreateInstance();

        currentIngredients = new List<Ingredient>();
    }

    private void Start()
    {
        GetUI();
    }

    private void Update()
    {
        CheckIfIngredientIsGettingDropped();

        CheckIfGasIsGettingDropped();
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

    private void CheckIfIngredientIsGettingDropped()
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
                float shortestDistance = 45;

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

                if (nearestMixingSlot != null)
                {
                    //Make sure the slot is active
                    nearestMixingSlot.EnableImage(true);

                    //Put the image of the item in the slot
                    nearestMixingSlot.Sprite = currentIngredient.GetSprite();

                    //Store the item properties in the slot
                    nearestMixingSlot.StoredIngredient = currentIngredient;

                    //Store the ingredient in the list
                    currentIngredients.Add(currentIngredient);

                    CheckForGasCreation();
                }

                //Drop the item
                currentIngredient = null;
            }
        }
    }

    private void CheckIfGasIsGettingDropped()
    {
        //When let go...
        if (Input.GetMouseButtonUp(0))
        {
            if (currentGas != null)
            {
                if (currentSlot == resultSlot)
                {
                    MoveGasFromTableToChest();
                }

                //Disable the image on the cursor
                cT.DisableCursorImage();
            }
        }
    }

    private void MoveGasFromTableToChest()
    {
        //Setup for dropping off the item
        InventorySlot nearestInventorySlot = null;
        float shortestDistance = 50;

        //Check which slot is the closest to the mouse cursor
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            float dist = Vector2.Distance(Input.mousePosition, inventorySlot.transform.position);

            //Friendly little contest between the slots lol
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearestInventorySlot = inventorySlot;
            }
        }

        if (nearestInventorySlot != null)
        {
            StoreGas(nearestInventorySlot);

            ClearMixingTable();
        }
    }

    private void CheckForGasCreation()
    {
        //Clears the slot
        ClearInventorySlot(resultSlot);

        //Try to craft using the ingredients currently in the chain
        if (CreateGas(currentIngredients) != null)
        {
            StoreGas(resultSlot);
        }
    }

    private Gas CreateGas(List<Ingredient> providedIngredients)
    {
        foreach (var recipe in recipes)
        {
            // Check if the provided ingredients match the recipe's ingredients
            if (HasIngredients(recipe, providedIngredients))
            {
                currentGas = recipe.ResultGas;
                return recipe.ResultGas;
            }
        }

        return null;
    }

    private bool HasIngredients(Recipe recipe, List<Ingredient> currentIngredients)
    {
        if (currentIngredients.Count > 1)
        {

            foreach (var recipeIngredient in recipe.Ingredients)
            {
                if (!currentIngredients.Contains(recipeIngredient))
                {
                    return false;
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    //Manages the dropping of ingredients into a slot
    public void DropIngredientInSlot(IngredientDisplay ingredientDisplay)
    {
        if (currentIngredient == null)
        {
            currentIngredient = ingredientDisplay.Ingredient;
            cT.ChangeActiveCursorImage(currentIngredient.GetSprite());
        }
    }

    public void DropGasInSlot(InventorySlot gasDisplay)
    {
        currentGas = gasDisplay.StoredGas;
        cT.ChangeActiveCursorImage(currentGas.GetSprite());
        currentSlot = gasDisplay;
    }

    private void ClearMixingTable()
    {        
        foreach (MixingSlot mixingslot in mixingSlots)
        {
            currentIngredients.Clear();
            ClearMixingSlot(mixingslot);
        }
    }

    private void StoreGas(InventorySlot storageSlot)
    {
        //Make sure the slot is active
        storageSlot.EnableImage(true);

        //Store the item properties in the slot
        storageSlot.StoredGas = currentGas;

        //Put the image of the item in the slot
        storageSlot.Sprite = currentGas.GetSprite();       

        //Clear gas
        currentGas = null;

        //Clear the slot we're using
        currentSlot = null;
    }

    private void ClearInventorySlot(InventorySlot inventorySlot)
    {
        inventorySlot.EnableImage(false);
        inventorySlot.Sprite = null;
        inventorySlot.StoredGas = null;        
        currentGas = null;
        currentSlot = null;
    }

    public void ClearMixingSlot(MixingSlot mixingSlot)
    {
        currentIngredients.Remove(mixingSlot.StoredIngredient);
        mixingSlot.StoredIngredient = null;
        mixingSlot.Sprite = null;
        mixingSlot.EnableImage(false);

        ClearInventorySlot(resultSlot);
    }
}
