using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScreenWrapping : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private Camera cam;
    
    [SerializeField] bool isWrappingX = false;
    [SerializeField] bool isWrappingY = false;

    bool CheckRenderers() { return renderers.Any(renderer => renderer.isVisible); } // if at least one renderer is visible, return true
    private void Awake() 
    { 
        if(renderers.Count <= 0) renderers.Add(GetComponent<Renderer>());
        if(cam == null) cam = FindObjectOfType<Camera>(); 
    } 
    private void Update() => ScreenWrap();

    private void ScreenWrap()
    {
        var isVisible = CheckRenderers();

        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY) return;

        var camera = cam;
        var position = transform.position;
        
        var viewportPosition = camera.WorldToViewportPoint(position);
        var newPosition = position;

        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        transform.position = newPosition;
    }
}
