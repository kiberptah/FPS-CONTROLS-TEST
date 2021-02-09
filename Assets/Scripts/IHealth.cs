using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    //float maxHealth { get; }
    //float currentHealth { get; }

    void onGettingHealth(Transform _whom, float _amount);
    void onLoosingHealth(Transform _who, Transform _whom, Vector3 _point, float _amount);
    void onNoHealth();

}
