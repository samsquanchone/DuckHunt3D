using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DuckAnimState {DEATH, FALLING}; //Only really need these two as it should just enter anim flying and exit on falling so just need to worry about transition to death to fallin
public class DuckAnimController : MonoBehaviour
{
    private Animator duckAnimController;

    private void Start()
    {
        duckAnimController = GetComponent<Animator>();
    }
    public void SetAnimState(DuckAnimState animState)
    {
        switch (animState)
        {
            case DuckAnimState.DEATH:
                duckAnimController.SetTrigger("Dead");
                break;

            case DuckAnimState.FALLING:
                duckAnimController.SetTrigger("Spin");
                break;
        }
    }
}
