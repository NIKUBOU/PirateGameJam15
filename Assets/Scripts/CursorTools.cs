using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorTools : MonoBehaviour
{
    private Image activeImage;

    private void Awake()
    {
        activeImage = GetComponentInChildren<Image>();

        TrackMouseCursor();
    }

    private void Update()
    {
        TrackMouseCursor();
    }

    private void TrackMouseCursor()
    {
        transform.position = Input.mousePosition;
    }

    //Changes the image displayed on the cursor when dragging an item
    public void ChangeActiveCursorImage(Image newCursorImage)
    {
        //Makes the image renderable
        activeImage.enabled = true;

        activeImage.sprite = newCursorImage.sprite;
    }

    public void DisableCursorImage()
    {
        activeImage.enabled = false;
    }
}
