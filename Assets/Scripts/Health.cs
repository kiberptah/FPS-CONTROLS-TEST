using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class Health : MonoBehaviour
{
    /*[SerializeField] private float maxHealth = 100f;
    private float health;


    void OnEnable()
    {
        EventDirector.someHeal += Heal;
        EventDirector.someAttack += TakeDamage;
        
    }
    private void OnDisable()
    {
        EventDirector.someHeal -= Heal;
        EventDirector.someAttack -= TakeDamage;

    }

    void Awake()
    {
        health = maxHealth;
    }
    private void Start()
    {
        EventDirector.player_updateHealth(transform, health);
    }


    void Heal(Transform _whom, float _amount)
    {
        if (_whom == transform)
        {
            ChangeHealth(_amount);
        }
    }
    void TakeDamage(Transform _who, Transform _whom, Vector3 _point, float _amount)
    {
        if (_whom == transform)
        {
            ChangeHealth(-_amount);

            if (health <= 0)
            {
                EventDirector.someDeath?.Invoke(gameObject.transform);
                Destroy(gameObject);
            }
        }
    }

    void ChangeHealth(float _amount)
    {
        health = Mathf.Clamp(health + _amount, 0, maxHealth);
        EventDirector.player_updateHealth(transform, health);
    }
*/
}
