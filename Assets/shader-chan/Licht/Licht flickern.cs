using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Notwendig für Light2D

public class Lichtflickern : MonoBehaviour
{
    private Light2D myLight;
    public float maxInterval = 1;
    public float maxFlicker = 0.2f;

    float defaultIntensity;
    bool isOn;
    float timer;
    float delay;

    private void Start()
    {
        myLight = GetComponent<Light2D>();
        if (myLight == null)
        {
            Debug.LogError("Keine Light2D-Komponente gefunden! Bitte fügen Sie eine Light2D-Komponente zu diesem GameObject hinzu.");
            enabled = false;
            return;
        }

        defaultIntensity = myLight.intensity;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        isOn = !isOn;

        if (isOn)
        {
            myLight.intensity = defaultIntensity;
            delay = Random.Range(0, maxInterval);
        }
        else
        {
            myLight.intensity = Random.Range(0.6f, defaultIntensity);
            delay = Random.Range(0, maxFlicker);
        }

        timer = 0;
    }
}
