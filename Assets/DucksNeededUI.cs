using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DucksNeededUI : MonoBehaviour
{
    
    //private TMP_Text duckNeededText;
    [SerializeField] GameObject barsContainer;

    [SerializeField] GameObject barGroupUIPrefab; //Roughly about one ducks worth of bars 
    List<Image> barSprites = new();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject _obj = Instantiate(barGroupUIPrefab, barsContainer.transform);
            //Add images to list here
        }
    }   

    int duckNeeded = 6; //Can remove this once we get the actual UI in!
    public void IncrementDucksNeeded()
    {
        duckNeeded += 1;
        //duckNeededText.SetText("Ducks neeed: " + duckNeeded);
        //Change transparencys ect ect!
    }
}
