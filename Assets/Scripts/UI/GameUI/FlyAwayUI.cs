using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyAwayUI : MonoBehaviour
{
    [SerializeField] private GameObject flyAwayUIObject;
    //Showing another way to do similar design as interface based observer pattern I have done for other classes
    UnityAction flashFlyAwayUI;
    UnityAction stopFlashingFlyAwayUI;

    bool hasDuckFlewAway = false;
    //Need Duck state is flying away start flashing, has flown away then stop flashing

    // Start is called before the first frame update
    void Start()
    {
        //Create actions
        flashFlyAwayUI += ShowDuckFlyAwayUI;
        stopFlashingFlyAwayUI += DisableDuckFlyAwayUI;

        //Add as listeners to their respective event!
        BroadCastManager.Instance.DuckFlyingAway.AddListener(flashFlyAwayUI);
        BroadCastManager.Instance.DuckFlownAway.AddListener(stopFlashingFlyAwayUI);
    }

    public void ShowDuckFlyAwayUI()
    {
        hasDuckFlewAway = false;
        flyAwayUIObject.SetActive(true);
        StartCoroutine(FlashUI());
    }

    public void DisableDuckFlyAwayUI()
    {
        hasDuckFlewAway = true;
    }

    IEnumerator FlashUI()
    {
        while (!hasDuckFlewAway)
        {
            flyAwayUIObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);


            flyAwayUIObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
        flyAwayUIObject.SetActive(false);
    }
}
