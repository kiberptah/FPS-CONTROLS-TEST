using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalFlickerController : MonoBehaviour
{
    public bool isOn;

    [SerializeField] private bool flickering = false;

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
        startOffset = Mathf.Clamp(startOffset + Random.Range(-randomStartSeconds, randomStartSeconds), 0, startOffset + randomStartSeconds);
    }


    private void Update()
    {
        if (flickering)
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
    }


    void Flick()
    {
        timer += Time.deltaTime;

        if (isOn == true && timer >= timeOn)
        {
            isOn = false;
            timer = 0;
        }
        if (isOn == false && timer >= timeOff)
        {
            isOn = true;
            timer = 0;
        }
    }

}
