using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHandler : MonoBehaviour
{
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void OnMouseEnter()
    {
        Debug.Log("Mouse entered");
        if (objectRenderer != null)
        {
            Debug.Log("Setting _isOutlineOn to 1");
            objectRenderer.material.SetInt("_istOutlineon", 1);
        }
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse exited");
        if (objectRenderer != null)
        {
            Debug.Log("Setting _isOutlineOn to 0");
            objectRenderer.material.SetInt("_istOutlineon", 0);
        }
    }
}
