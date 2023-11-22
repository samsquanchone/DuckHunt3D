using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareDuck :  Duck, IPlayerObserver, IDuckSubject
{
    

    //Can't use polymorphism on this interface function, so remember to add if making new duck types!
    void IPlayerObserver.OnNotify(PlayerState state)
    {
        if (this.isActiveAndEnabled) //Could use ID but this should work fine, avoids calling behaviour on all pooled objects
        {
            playerState = state;
            switch (state)
            {
                case PlayerState.DUCK_SHOT:
                    CalculateScore();
                    StartCoroutine(DespawnTimer());
                    break;
            }
        }
    }

    protected override void CalculateScore()
    {
        
        int score = Maths.CalculateBirdShotScore(timeOnScreen, movementSpeed) + 500; //Just give a nice bonus for shooting a rare bird!
        NotifyObservers(score, new Vector2(this.transform.position.x, this.transform.transform.position.y));
      // GameManager.Instance.IncrementScore(score, new Vector2(this.transform.position.x, this.transform.transform.position.y));
    }
}
