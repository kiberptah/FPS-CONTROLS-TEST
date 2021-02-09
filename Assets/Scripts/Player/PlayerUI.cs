using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text healthPoints;
    private void OnEnable()
    {
        EventDirector.player_updateHealth += UpdateHealthPointsUI;
    }
    private void OnDisable()
    {
        EventDirector.player_updateHealth -= UpdateHealthPointsUI;

    }

    void UpdateHealthPointsUI(float _playerHealth)
    {
        healthPoints.text = Mathf.Round(_playerHealth).ToString();
    }
}
