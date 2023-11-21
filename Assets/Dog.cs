using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    Animator dogAnim;
    float timeElapsed;
    float lerpDuration = 2;
    float startValue = 0;
    float endValue = 3.5f;
    float valueToLerp;


    Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        dogAnim = GetComponent<Animator>();
        targetPos = new Vector3(-15f, transform.position.y, transform.position.z);
        StartCoroutine(Lerp());
    }

    private void Update()
    {
        float step = valueToLerp * Time.deltaTime;
        dogAnim.SetFloat("Move", valueToLerp);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
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
}
