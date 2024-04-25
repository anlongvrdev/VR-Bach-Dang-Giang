using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHide : MonoBehaviour
{
    // Reference to the GameObject you want to show/hide
    public GameObject itemToShowHide;

    // Flag to track the visibility state of the item
    private bool isVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide the item
        itemToShowHide.SetActive(false);
    }

    // Function to toggle the visibility of the item
    public void ToggleItemVisibility()
    {
        // Toggle the visibility state
        isVisible = !isVisible;

        // Set the visibility of the item based on the state
        itemToShowHide.SetActive(isVisible);
    }
}
