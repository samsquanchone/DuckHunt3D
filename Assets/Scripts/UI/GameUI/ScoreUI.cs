using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ScoreUI : MonoBehaviour
{
    private TMP_Text overalScoreText;
    const string overalScorePrefix = "Score: ";

    [SerializeField] TMP_Text popUpScoreText;
    const string popUpScorePrefix = "+";

    private void Start()
    {
        overalScoreText = GetComponent<TMP_Text>(); //Get ref to TMP text
        overalScoreText.SetText(overalScorePrefix + 0);
    }

    public void SetScore(int score)
    {
       
        overalScoreText.SetText(overalScorePrefix + score);
    }

    public void SetPopUpScore(int score, Vector2 pos)
    {
        popUpScoreText.SetText(popUpScorePrefix + score);
        popUpScoreText.transform.position= pos;
        popUpScoreText.gameObject.SetActive(true);

        StartCoroutine(PopUpTimer());
       
    }

    IEnumerator PopUpTimer()
    {
        yield return new WaitForSeconds(0.5f);
        popUpScoreText.gameObject.SetActive(false);
    }
}
