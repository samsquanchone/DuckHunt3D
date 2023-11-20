using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DuckCountUI : MonoBehaviour, IRoundObserver
{
    const string prefix = "Duck Count: ";
    const string suffix = "/10";

    Color blackCol = Color.black;
    Color redCol = Color.red;
    Color whiteCol = Color.white;
    Color alphaColour;
    [SerializeField] private GameObject duckPrefab;
    [SerializeField] private GameObject duckContainer;
    private List<Image> duckSpriteList = new();

    [SerializeField] private TMP_Text duckCountText; //Just for prototyping
    private int duckCount = 0; //very cheecky should deffo change!

    float flashDuration = 2f;
    bool isSpawnInterim = false; //Should be spawning bird

    bool isRoundInterim = false;
    // Start is called before the first frame update
    void Start()
    {
        BroadCastManager.Instance.AddRoundObserver(this); //Subscribe to round subject
        //Could replace hard increment by a number for e.g. implementing different game modes!
        for (int i = 0; i < 10; i++)
        {
            //Spawn duck UI and add to list
            GameObject _obj = Instantiate(duckPrefab, duckContainer.transform);
            duckSpriteList.Add(_obj.GetComponent<Image>());
            duckSpriteList[i].color = whiteCol;
        }
        alphaColour = duckSpriteList[0].color; //Cache alpha colour
    }


    public void NewDuckSpawning()
    {
        isSpawnInterim = true;
        StartCoroutine("FlashDuck");
    }


    public void DuckHit()
    {
        duckSpriteList[duckCount].color = redCol;
        IncrementDuckCount();
    }

    public void DuckMissed() //Flew away
    {
        duckSpriteList[duckCount].color = blackCol;
        IncrementDuckCount();
    }

    void IncrementDuckCount()
    {
        if (duckCount + 1 < duckSpriteList.Count)
        {
            duckCount += 1;
        }
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
        if (!isRoundInterim)
        {
           while (isSpawnInterim)
            {
                yield return new WaitForSeconds(0.5f);
                alphaColour.a = 0;
                duckSpriteList[duckCount].color = alphaColour;

                yield return new WaitForSeconds(0.5f);
                alphaColour.a = 1;
                duckSpriteList[duckCount].color = alphaColour;

                yield return new WaitForSeconds(flashDuration);
                isSpawnInterim = false;


            }
        }
        
    }
   

    IEnumerator RoundInterimFlash()
    {
        while (isRoundInterim)
        {
            foreach (var duck in duckSpriteList)
            {
                alphaColour.a = 0;
                duck.color = whiteCol;
            }
            yield return new WaitForSeconds(0.2f);
            foreach (var duck in duckSpriteList)
            {
                alphaColour.a = 1;
                duck.color = redCol;
            }
            yield return new WaitForSeconds(0.2f);
        }

        ResetDuckCount();
    }

    void IRoundObserver.OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
    {
        switch (state)
        {
            case RoundState.DUCKSPAWNINTERIM:

                break;

            case RoundState.DUCKSPAWNING:
                NewDuckSpawning();
                break;

            case RoundState.NEWROUND:
                isRoundInterim = false;
                ResetDuckCount();
                break;
            case RoundState.BIRDHIT:
                DuckHit();
                break;
            case RoundState.BIRDFLYAWAY:
                DuckMissed();
                break;

            case RoundState.ROUNDINTERIM:
                isRoundInterim = true;
                StartCoroutine(RoundInterimFlash());
                break;
                
        }
    }
}
