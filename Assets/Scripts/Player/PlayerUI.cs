using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Features")]
    public bool showHealth;
    public bool showTakeDamageFlash;

    [Header("Tweaking")]
    float takeDamageFlashDuration = 1f;
    float takeDamageFlash_maxIntensity;


    [Header("References")]
    public Text healthPoints;
    public Image takeDamageFlash;
    
    private void OnEnable()
    {
        EventDirector.player_updateHealth += UpdateHealthPointsUI;
        EventDirector.player_TakeDamage += TakeDamageScreenFlashing;

    }
    private void OnDisable()
    {
        EventDirector.player_updateHealth -= UpdateHealthPointsUI;
        EventDirector.player_TakeDamage -= TakeDamageScreenFlashing;


    }
    private void Awake()
    {
        takeDamageFlash_maxIntensity = takeDamageFlash.color.a;
    }
    void TakeDamageScreenFlashing(float _amount)
    {
        if (showTakeDamageFlash)
        {
            StopCoroutine(ScreenFlash());
            StartCoroutine(ScreenFlash());
        }
    }
    IEnumerator ScreenFlash()
    {
        takeDamageFlash.gameObject.SetActive(true);
        takeDamageFlash.color = new Color(takeDamageFlash.color.r, takeDamageFlash.color.g, takeDamageFlash.color.b, takeDamageFlash_maxIntensity);

        float timestep = 0.01f;
        float timePassed = 0;

        //float timestepsAmount = takeDamageFlashDuration / timestep;
        float colorDecreaseStep = takeDamageFlash.color.a / takeDamageFlashDuration * timestep;

        while (timePassed < takeDamageFlashDuration)
        {
            timePassed += Time.deltaTime;
            takeDamageFlash.color = new Color(takeDamageFlash.color.r, takeDamageFlash.color.g, takeDamageFlash.color.b, 
                takeDamageFlash.color.a - colorDecreaseStep);

            yield return new WaitForSeconds(timestep);
        }

        takeDamageFlash.gameObject.SetActive(false);

        yield return null;
    }


    void UpdateHealthPointsUI(float _playerHealth)
    {
        if (showHealth)
        {
            healthPoints.text = Mathf.Round(_playerHealth).ToString();
        }
    }
}
