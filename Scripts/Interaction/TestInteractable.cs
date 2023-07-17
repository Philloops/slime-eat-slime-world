using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public GameObject interactVisual;

    public void OnHoverEnter()
    {
        Debug.Log("OnHoverEnter thru -> " + transform.name);
        interactVisual.SetActive(true);
    }
    public void OnHoverExit()
    {
        Debug.Log("OnHoverExit thru -> " + transform.name);
        interactVisual.SetActive(false);
    }
    public void Interact()
    {
        Debug.Log("Interact thru -> " + transform.name);
    }
}
