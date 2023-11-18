using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Duck : MonoBehaviour, IPlayerObserver
{

    [SerializeField] private float speed;
    private Vector3 nextPosition; //Could use transform, but we only need the .position so we can just use a vector 3

    private float duration = 5f;
    float timeOnScreen = 0;

    UnityAction birdFlewAway;
    // Start is called before the first frame update
    void Start()
    {
        Maths.SetBounds(new System.Numerics.Vector2(-5, 5), new System.Numerics.Vector2(0, 10), new System.Numerics.Vector2(-7, 7)); //Need to move to game manager!
        birdFlewAway += () => SpawnManager.Instance.DespawnBird(); //Create an action that will notify spawn manager to despawn current bird when it flys away!
        //GetNextFlyToPosition();
    }

    //Called when pool is filled as we want to add this as player observer, but is being set to inactive so failing to register as observer1
    public void Innit()
    {

        GameManager.Instance.AddPlayerObserver(this); //Due to the pooling manager this is only done once on game scene start :)
    }

    private void OnEnable()
    {
        duration = 5f;
        timeOnScreen = 0;
        GetNextFlyToPosition();
        StartCoroutine("FlyAwayTimer");
    }

    //Fixed update for smoother movement
    void FixedUpdate()
    {
        timeOnScreen =+ Time.deltaTime;
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, nextPosition) < 0.001f)
        {
            //Calculate a new position
            GetNextFlyToPosition();

        }
    }

    private void GetNextFlyToPosition()
    {
        nextPosition.x = Maths.GetRandomPosition3D().X;
        nextPosition.y = Maths.GetRandomPosition3D().Y;
        nextPosition.z = Maths.GetRandomPosition3D().Z;
    }

    public void FlyAway()
    {
        birdFlewAway.Invoke();
    }

    private void CalculateScore()
    {
        GameManager.Instance.IncrementScore(Maths.CalculateBirdShotScore(timeOnScreen, speed));
    }

    IEnumerator FlyAwayTimer()
    {
        yield return new WaitForSeconds(duration);
        FlyAway();
        
    }

    public void OnNotify(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.DUCK_SHOT:
                CalculateScore();
                break;
        }
    }
}
