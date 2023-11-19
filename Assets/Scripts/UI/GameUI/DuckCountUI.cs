using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DuckCountUI : MonoBehaviour
{
    const string prefix = "Duck Count: ";
    const string suffix = "/10";

    Color blackCol = Color.black;
    Color redCol = Color.red;
    Color whiteCol = Color.white;
    [SerializeField] private GameObject duckPrefab;
    [SerializeField] private GameObject duckContainer;
    private List<Image> duckSpriteList = new();

    [SerializeField] private TMP_Text duckCountText; //Just for prototyping
    private int duckCount = 0; //very cheecky should deffo change!

    float flashDuration = 2f;

    bool isNewRound = false; //Should be spawning bird
    // Start is called before the first frame update
    void Start()
    {
        //Could replace hard increment by a number for e.g. implementing different game modes!
        for (int i = 0; i < 10; i++)
        {
            //Spawn duck UI and add to list
            GameObject _obj = Instantiate(duckPrefab, duckContainer.transform);
            duckSpriteList.Add(_obj.GetComponent<Image>());
            duckSpriteList[i].color = whiteCol;
        }
      //  duckCountText.SetText(prefix + duckCount + suffix);
    }


    public void NewDuckSpawning()
    {
        isNewRound = true;
        StartCoroutine("FlashDuck");

    }
    

    public void DuckHit()
    {
        duckCount += 1;
        duckSpriteList[duckCount - 1].color = redCol;
    }

    public void DuckMissed() //Flew away
    {
        duckCount += 1;
        duckSpriteList[duckCount - 1].color = blackCol;
    }

    //Maybe have duck missed / duck hit to fill or leave blank with an array of images! Could have hit fill in 

    public void ResetDuckCount()
    {
        duckCount = 0;
        foreach (var duck in duckSpriteList)
        {
            duck.color = whiteCol;
        }
       // duckCountText.SetText(prefix + duckCount + suffix);
    }

    IEnumerator FlashDuck()
    {
        Color alphaColour = duckSpriteList[duckCount].color;
        while (isNewRound)
        {
            yield return new WaitForSeconds(0.5f);
            alphaColour.a = 0;
            duckSpriteList[duckCount].color = alphaColour;

            yield return new WaitForSeconds(0.5f);
            alphaColour.a = 1;
            duckSpriteList[duckCount].color = alphaColour;

            yield return new WaitForSeconds(flashDuration);
            isNewRound = false;


        }
    }
}
