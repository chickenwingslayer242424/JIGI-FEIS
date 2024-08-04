using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lightmover : MonoBehaviour
{
    [SerializeField] private Light2D light;

    private int frames = 0;

    [SerializeField] private int framesPerMove;
    
    [SerializeField] private float moveRange;
    [SerializeField] private float moveSpeed;

    private float initialY;
    private float initialX;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the light's initial x and y position
        initialX = light.transform.position.x;
        initialY = light.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        frames++;
        if (frames % framesPerMove == 0)
        { 
            MoveLight();
        }
    }

    void MoveLight()
    {
        float newX = initialX + Mathf.Sin(Time.time * moveSpeed) * moveRange;
        Vector3 pos = light.transform.position;
        pos.x = newX;
        pos.y = initialY; // Ensure the light stays at the initial y position
        light.transform.position = pos;
    }
}
