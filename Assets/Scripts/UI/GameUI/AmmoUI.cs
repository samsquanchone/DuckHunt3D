using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour, IPlayerObserver, IRoundObserver
{
   
    [SerializeField] private List<GameObject> ammoUIObjects = new();
    int ammoCount = 2; //Start from 2 as we will have list as 0 index start



    Color alphaColour;

    void Start()
    {
        BroadCastManager.Instance.AddPlayerObserver(this);
        BroadCastManager.Instance.AddRoundObserver(this);
        alphaColour = ammoUIObjects[0].GetComponent<Image>().color;
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


    void IPlayerObserver.OnNotify(PlayerState state)
    {
        //Don't really need a switch case here as we want to notify when we miss and hit to decrease the ammo count
        DecreaseAmmoCount();
    }

    void IRoundObserver.OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
    {
        switch (state)
        {
           
            case RoundState.DUCKSPAWNING:
                ResetAmmo();
                break;

            case RoundState.NEWROUND:
                ResetAmmo();
                break;
           
        }
    }
}
