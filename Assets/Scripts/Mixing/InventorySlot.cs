using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //Can be fed an item and it will keep it
    private Gas storedGas;
    public Gas StoredGas { get { return storedGas; } set { storedGas = value; } }

    private Image image;
    public Sprite Sprite { set { image.sprite = value; } }

    private Plane plane;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        plane = GetComponent<Plane>();
    }

    public void EnableImage(bool active)
    {
        image.enabled = active;
    }

    public void OnEquip()
    {
        plane.ApplyGasEffects(storedGas);
        plane.StartChemTrail();
    }
}
