using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    Animator weaponAnimator;
    public ParticleSystem muzzleFlash;
    private void OnEnable()
    {
        EventDirector.player_drawWeapon += drawWeapon;
        EventDirector.player_holsterWeapon += holsterWeapon;
        EventDirector.player_firing += fireWeapon;

    }
    private void OnDisable()
    {
        EventDirector.player_drawWeapon -= drawWeapon;
        EventDirector.player_holsterWeapon -= holsterWeapon;
        EventDirector.player_firing -= fireWeapon;


    }
    private void Awake()
    {
        weaponAnimator = GetComponent<Animator>();
    }
    public void changeWeaponStatusToHolstered()
    {
        //isWeaponHolstered = true;
        EventDirector.player_isWeaponDrawn(false);
    }
    public void changeWeaponStatusToDrawn()
    {
        //isWeaponHolstered = false;
        EventDirector.player_isWeaponDrawn(true);
    }

    void drawWeapon()
    {
        weaponAnimator?.CrossFade("weapon_draw", 0.5f);
    }
    
    void holsterWeapon()
    {
        weaponAnimator?.CrossFade("weapon_holster", 0.5f);
    }
    
    void fireWeapon()
    {
        //Debug.Log("fire anim");

        weaponAnimator?.Play("weapon_fire");
    }

    void triggerMuzzleFlash()
    {
        muzzleFlash.Play();
    }
}
