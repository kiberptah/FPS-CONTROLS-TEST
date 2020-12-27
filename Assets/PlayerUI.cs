using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text healthPoints;
    private Health health;

    void Awake()
    {
        if (transform.GetComponent<Health>() != null)
        {
            health = transform.GetComponent<Health>();
        }
        else
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthPointsUI();
    }

    void UpdateHealthPointsUI()
    {
        healthPoints.text = health.health.ToString();
    }
}
