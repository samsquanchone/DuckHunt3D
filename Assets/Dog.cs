using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour, IRoundObserver
{
    Animator dogAnim;
   
    float lerpDuration = 2;
    float startValue = 0;
    float endValue = 3.5f;
    float valueToLerp;
    float xPostitionToInvert = -10;


    Vector3 targetPos;
    private bool hasRotated = false;
    // Start is called before the first frame update
    void Start()
    {
        BroadCastManager.Instance.AddRoundObserver(this);
        dogAnim = GetComponent<Animator>();
        targetPos = new Vector3(-10f, transform.position.y, transform.position.z);
        StartCoroutine(Lerp());
    }

    private void Update()
    {
        float step = valueToLerp * Time.deltaTime;
        dogAnim.SetFloat("Move", valueToLerp);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if (Vector3.Distance(transform.position, targetPos) < 0.001f && !hasRotated)
        {
            transform.rotation = Quaternion.Inverse(transform.rotation); //Invert x rotation as al others are 0
            hasRotated = true;
            valueToLerp = 0;
        
        }
    }



    void RunDogAcrossScreen()
    {
       
         xPostitionToInvert = -xPostitionToInvert; //Invert x pos
       
        targetPos = new Vector3(xPostitionToInvert, transform.position.y, transform.position.z);
        valueToLerp = 3.5f;
        hasRotated = false;
        StartCoroutine(Lerp());
    
    }

    IEnumerator Lerp()
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        valueToLerp = endValue;
      
        
    }

    public void OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
    {
        switch (state)
        {
            case RoundState.ROUNDINTERIM:
                RunDogAcrossScreen();
                break;
        }
    }
}
