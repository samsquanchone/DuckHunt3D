using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DucksNeededUI : MonoBehaviour, IRoundObserver
{

    //private TMP_Text duckNeededText;
    [SerializeField] GameObject barsContainer;

    [SerializeField] GameObject barGroupUIPrefab; //Roughly about one ducks worth of bars 
    List<GameObject> barsObjectList = new();

    Color alphaColour;

    int duckStartNumber = 6; //Could change depending on game mode ect
    int duckNeeded = 6; //Can remove this once we get the actual UI in!

    private void Start()
    {
        BroadCastManager.Instance.AddRoundObserver(this);
        alphaColour = barGroupUIPrefab.GetComponentInChildren<Image>().color;
        alphaColour.a = 0;
        for (int i = 0; i < 10; i++)
        {
            GameObject _obj = Instantiate(barGroupUIPrefab, barsContainer.transform);
            barsObjectList.Add(_obj);
            if (i > 5)
            {
                Image[] bars = _obj.GetComponentsInChildren<Image>();
                foreach (var bar in bars)
                {
                    bar.color = alphaColour;
                }
            }
        }

        alphaColour.a = 1; //Reset alpha to 1 here as we will only be 'adding bars' not removing them, so this avoids having to re-assign
    }


    public void IncrementDucksNeeded()
    {
        duckNeeded += 1;

        //Potential issue with 9th bird, may be one out with duck needed, need to test to double check!
        Image[] bars = barsObjectList[duckNeeded - 1].GetComponentsInChildren<Image>();
        foreach (var bar in bars)
        {
            bar.color = alphaColour;
        }

        StartCoroutine("FlashBarGroup"); //Flash new bar group that appears
    }

    IEnumerator FlashBarGroup()
    {

        yield return new WaitForSeconds(0.5f);
        alphaColour.a = 0;
        Image[] bars = barsObjectList[duckNeeded - 1].GetComponentsInChildren<Image>();
        foreach (var bar in bars)
        {
            bar.color = alphaColour;
        }

        yield return new WaitForSeconds(0.5f);
        alphaColour.a = 1;

        foreach (var bar in bars)
        {
            bar.color = alphaColour;
        }

    }

    public void OnNotify(RoundState state, int _currentRound, int _birdsNeeded)
    {
        switch (state)
        {
                case RoundState.DUCKSNEEDEDINCREASED:
                IncrementDucksNeeded();
                break;

        }
    }
}
