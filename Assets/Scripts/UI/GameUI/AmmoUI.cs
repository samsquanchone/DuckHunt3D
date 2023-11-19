using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour, IPlayerObserver
{
    
   
    [SerializeField] private List<GameObject> ammoUIObjects = new();
    int ammoCount = 2; //Start from 2 as we will have list as 0 index start

    Color alphaColour;

    void Start()
    {
        GameManager.Instance.AddPlayerObserver(this);
        alphaColour = ammoUIObjects[0].GetComponent<Image>().color;
    }

    public void OnNotify(PlayerState state)
    {
        //Don't really need a switch case here as we want to notify when we miss and hit to decrease the ammo count
        DecreaseAmmoCount();
    }

    private void DecreaseAmmoCount()
    {
        //As I am using a layoutgroup setting to not active resized objects, hence why I am using this alpha method
        alphaColour.a = 0;
        ammoUIObjects[ammoCount].GetComponent<Image>().color = alphaColour;
        ammoCount -= 1;
    }

    public void ResetAmmo()
    {
        foreach (var ammo in ammoUIObjects)
        {
            alphaColour.a = 1;
            ammo.GetComponent<Image>().color = alphaColour;
        }

        ammoCount = 2;
    }

}
