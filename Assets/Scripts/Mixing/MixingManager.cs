using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private CursorTools cT;

    //Variables
    private Ingredient currentIngredient;
    private List<Ingredient> currentIngredients;
    private Gas currentGas;

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
                    //Empty slot
                    ClearMixingSlot(nearestMixingSlot);

                    //Make sure the slot is active
                    nearestMixingSlot.EnableImage(true);

                    //Put the image of the item in the slot
                    nearestMixingSlot.Sprite = currentIngredient.Logo;

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
                //Setup for dropping off the item
                InventorySlot nearestInventorySlot = null;
                float shortestDistance = 4;

                //Find all active planes
                List<InventorySlot> currentPlanes = new List<InventorySlot>(FindObjectsOfType<InventorySlot>());

                // Forget the y transform of the mousePosition for the contest
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Camera.main.nearClipPlane; // Use near clip plane to convert screen point to world point
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                // Check which slot is the closest to the mouse cursor
                foreach (InventorySlot inventorySlot in currentPlanes)
                {
                    Vector3 slotPosition = inventorySlot.transform.position;
                    slotPosition.y = 10; // Ignore the Y-axis for comparison purposes
                    mouseWorldPosition.y = 10; // Ensure both points are at the same height for accurate 2D distance

                    float dist = Vector3.Distance(mouseWorldPosition, slotPosition);

                    // Friendly little contest between the slots lol
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

                //Disable the image on the cursor
                cT.DisableCursorImage();
            }
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
                ApplyGasEffects(currentGas, recipe);
                return recipe.ResultGas;
            }
        }

        return null;
    }

    private void ApplyGasEffects(Gas appliedGas, Recipe appliedRecipe)
    {
        //Clear effects
        appliedGas.NeighboringTileCall = 0;
        appliedGas.Rotation = 0;
        appliedGas.SpeedMultiplyer = 1;

        //Apply ingredient effects to the gas
        foreach (Ingredient ingredient in appliedRecipe.Ingredients)
        {
            appliedGas.NeighboringTileCall += ingredient.NeighboringTileCalls;
            appliedGas.Rotation += ingredient.Rotation;
            appliedGas.SpeedMultiplyer = ingredient.SpeedMultiplyer;
        }
    }

    private bool HasIngredients(Recipe recipe, List<Ingredient> currentIngredients)
    {
        // Create a copy of the current ingredients list to avoid modifying the original list
        List<Ingredient> ingredientsCopy = new List<Ingredient>(currentIngredients);

        // Check if the recipe ingredients are all in the current ingredients
        foreach (var recipeIngredient in recipe.Ingredients)
        {
            // Try to find a matching ingredient in the current ingredients copy
            if (ingredientsCopy.Contains(recipeIngredient))
            {
                // Remove the ingredient from the copy to account for multiple instances
                ingredientsCopy.Remove(recipeIngredient);
            }
            else
            {
                // If any ingredient is not found, return false
                return false;
            }
        }

        // If all recipe ingredients are found, return true
        return true;
    }

    //Manages the dropping of ingredients into a slot
    public void DropIngredientInSlot(IngredientDisplay ingredientDisplay)
    {
        if (currentIngredient == null)
        {
            currentIngredient = ingredientDisplay.Ingredient;
            cT.ChangeActiveCursorImage(currentIngredient.Logo);
        }
    }

    public void DropGasInSlot(InventorySlot gasDisplay)
    {
        currentGas = gasDisplay.StoredGas;
        cT.ChangeActiveCursorImage(currentGas.Logo);
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
        storageSlot.Sprite = currentGas.Logo;

        if (storageSlot != resultSlot)
        {
            //Equips the gasses' effects
            storageSlot.OnEquip();
        }

        //Clear gas
        currentGas = null;
    }

    private void ClearInventorySlot(InventorySlot inventorySlot)
    {
        inventorySlot.EnableImage(false);
        inventorySlot.Sprite = null;
        inventorySlot.StoredGas = null;        
        currentGas = null;
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
