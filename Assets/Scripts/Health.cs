using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class Health : MonoBehaviour
{
    
    private float health;
    [SerializeField] private float maxHealth = 100f;


    void OnEnable()
    {
        EventDirector.someHeal += Heal;
    }
    private void OnDisable()
    {
        EventDirector.someHeal -= Heal;

    }

    void Awake()
    {
        health = maxHealth;
    }
    private void Start()
    {
        EventDirector.updateHealth(transform, health);
    }
    void Update()
    {
        if (health <= 0)
        {
            EventDirector.someDeath?.Invoke(gameObject.transform);
            Destroy(gameObject);
        }
    }



    public void TakeDamage(float _damage)
    {
        health = Mathf.Clamp(health - _damage, 0, maxHealth);
        EventDirector.updateHealth(transform, health);
    } 

    void Heal(float _amount)
    {
        health = Mathf.Clamp(health + _amount, 0, maxHealth);
        EventDirector.updateHealth(transform, health);
    }



}
