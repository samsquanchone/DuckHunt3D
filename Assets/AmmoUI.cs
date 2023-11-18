using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour, IPlayerObserver
{
    
    const string prefix = "AMMO: ";
    int ammo = 3; //Wil be using until we have bullet images (As we are keeping track of ammo for round purposes, and i don't like repeating this here)

    [SerializeField] private TMP_Text ammoText;

    void Start()
    {
        GameManager.Instance.AddPlayerObserver(this);

        ammoText.SetText(prefix + ammo);
    }

    public void OnNotify(PlayerState state)
    {
        //Don't really need a switch case here as we want to notify when we miss and hit to decrease the ammo count
        DecreaseAmmoCount();
    }

    private void DecreaseAmmoCount()
    {
        ammo -= 1;
        ammoText.SetText(prefix + ammo);
    }

    public void ResetAmmo()
    {
        ammo = 3;
        ammoText.SetText(prefix + ammo);
    }

}
