using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotUI : MonoBehaviour, IPlayerObserver
{
    private Image blackScreen;
    Color alphaColour;

    void Start()
    {
        BroadCastManager.Instance.AddPlayerObserver(this);
        blackScreen = GetComponent<Image>();
        alphaColour = blackScreen.color;

    }

    void IPlayerObserver.OnNotify(PlayerState state)
    {
        StartCoroutine(FlashScreenBlack());
    }

    IEnumerator FlashScreenBlack()
    {
        alphaColour.a = 1;
        blackScreen.color = alphaColour;

        yield return new WaitForSecondsRealtime(0.05f);

        alphaColour.a = 0;
        blackScreen.color = alphaColour;

    }
}
