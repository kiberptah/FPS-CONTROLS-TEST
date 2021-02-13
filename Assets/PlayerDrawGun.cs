using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerDrawGun : MonoBehaviour
{
    public GameObject weaponOfChoice;
    public float timeToDraw = 0.5f;
    Vector3 drawnWeaponPosition;
    Vector3 hiddenWeaponPosition;

    bool isWeaponDrawn = false;
    //Animator weaponAnimator;
    

    Vector3 offsetWeaponPositionToHide = new Vector3(0, -0.5f, 0);

    private void Start()
    {
        drawnWeaponPosition = weaponOfChoice.transform.localPosition;
        hiddenWeaponPosition = drawnWeaponPosition + offsetWeaponPositionToHide;


        //weaponOfChoice.TryGetComponent<Animator>(out weaponAnimator);


        ChangeWeaponStatus(false);
    }

    private void Update()
    {
        WeaponController();
    }

    void WeaponController()
    {
        if (Input.GetButtonDown("DrawWeapon"))
        {
            ChangeWeaponStatus(true);
        }
        if (Input.GetButtonUp("DrawWeapon"))
        {
            ChangeWeaponStatus(false);
        }
    }

    void ChangeWeaponStatus(bool _isWeaponDrawn)
    {
        if (_isWeaponDrawn)
        {
            //weaponOfChoice.transform.localPosition = drawnWeaponPosition;
            //weaponAnimator?.CrossFade("weapon_draw", 0.15f);
            EventDirector.player_drawWeapon?.Invoke();
        }
        else
        {
            //weaponOfChoice.transform.localPosition = hiddenWeaponPosition;
            //weaponAnimator?.CrossFade("weapon_holster", 0.15f);
            EventDirector.player_holsterWeapon?.Invoke();
        }
    }
}
