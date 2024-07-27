using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Image image;
    public Sprite Sprite { set { image.sprite = value; } }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void EnableImage(bool active)
    {
        image.enabled = active;
    }
}
