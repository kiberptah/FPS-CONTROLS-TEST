using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text healthPoints;
    private Health health;

    private void OnEnable()
    {
        EventDirector.updateHealth += UpdateHealthPointsUI;
    }
    private void OnDisable()
    {
        EventDirector.updateHealth -= UpdateHealthPointsUI;

    }

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
        
    }

    void UpdateHealthPointsUI(Transform _maybePlayer, float _playerHealth)
    {
        if (_maybePlayer == Player.singleton.transform)
        {
            healthPoints.text = Mathf.Round(_playerHealth).ToString();
        }
    }
}
