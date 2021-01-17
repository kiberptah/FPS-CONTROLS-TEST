using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterController : MonoBehaviour
{
    [Header("External-Controlled Flickering")]
    [SerializeField] private bool isExternallyControlled = false;
    [SerializeField] private ExternalFlickerController externalController;
    [SerializeField] private bool isOn;
    [Header("Self-Controlled Flickering")]
    [SerializeField] private bool flickering = false;
    [SerializeField] private GameObject emitter;
    

    [SerializeField] private float startOffset;
    [SerializeField] private float randomStartSeconds = 0; //seconds

    [SerializeField] private float timeOn;
    [SerializeField] private float timeOff;
    //[SerializeField] private float randomFlickPercent = 0; // 0 - 1

    private bool canStart = false;
    private float timeSinceAwake = 0;

    private float timer = 0;

    private void Awake()
    {
        if (emitter == null)
        {
            emitter = transform.GetChild(0).transform.gameObject;
        }

        startOffset = Mathf.Clamp(startOffset + Random.Range(-randomStartSeconds, randomStartSeconds), 0, startOffset + randomStartSeconds);
    }


    private void Update()
    {
        if (flickering && !isExternallyControlled)
        {
            if (canStart)
            {
                Flick();
            }
            else
            {
                timeSinceAwake += Time.deltaTime;
                if (timeSinceAwake >= startOffset)
                {
                    canStart = true;
                }
            }
        }
        if (isExternallyControlled)
        {
            ExternallyControlledFlick();
        }
    }


    void Flick()
    {
        timer += Time.deltaTime;

        if (emitter.activeSelf == true && timer >= timeOn)
        {
            emitter.SetActive(false);
            timer = 0;
        }
        if (emitter.activeSelf == false && timer >= timeOff)
        {
            emitter.SetActive(true);
            timer = 0;
        }
    }

    void ExternallyControlledFlick()
    {
        emitter.SetActive(externalController.isOn);
    }
}
