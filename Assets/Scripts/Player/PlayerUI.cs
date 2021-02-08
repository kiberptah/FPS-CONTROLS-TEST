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
        EventDirector.player_updateHealth += UpdateHealthPointsUI;
    }
    private void OnDisable()
    {
        EventDirector.player_updateHealth -= UpdateHealthPointsUI;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateHealthPointsUI(float _playerHealth)
    {
        //Debug.Log("UPDATE HP");
        healthPoints.text = Mathf.Round(_playerHealth).ToString();
    }
}
