using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Utility.Math;
using Utility.Broadcast;
using Utility.Pooling;


namespace GamePlay.Spawn
{

    /*Called a manager as it was originally using the singleton pattern, however through some refactoring no static instance access were required with this script
     * Singletons are a popular pattern in game design, but they do encourage coupling, which is why I would choose another pattern if possible
    */


    /// <summary>
    /// This script handles getting and returning objects to the pool, and does this through accessing the pooling manager singleton
    /// It is notified by the broadast manager Duckflown away and duck dead, which trigger the spawn bird function.
    /// It is also notified by the interface bsaed observer approach, being IRoundObserver, notifying this class when duck should spawn. 
    /// </summary>

    public class SpawnManager : MonoBehaviour, IRoundObserver
    {

        GameObject _obj; //Current bird active 

        UnityAction DespawnBirdAction; //Action for spawning bird

        PoolingObjectType poolingObjectType; //Used to determine which duck to spawn


        private void Start()
        {
            DespawnBirdAction += DespawnBird;

            //Set up what this class will listen to 
            BroadCastManager.Instance.AddRoundObserver(this);
            BroadCastManager.Instance.DuckFlownAway.AddListener(DespawnBirdAction);
            BroadCastManager.Instance.DuckDead.AddListener(DespawnBirdAction);
        }

        public void SpawnBird()
        {
            float randomNumber = Random.Range(0, 20);
            if (randomNumber == 19)
            {
                poolingObjectType = PoolingObjectType.RAREDUCK;

            }
            else
            {
                poolingObjectType = PoolingObjectType.DUCK;
            }
            StartCoroutine(WaitToSpawn(poolingObjectType));
        }
        IEnumerator WaitToSpawn(PoolingObjectType type)
        {
            yield return new WaitForSeconds(0.1f);

            //Get object from pool
            _obj = PoolingManager.Instance.GetPoolObject(type);

            //Calculate spawn position
            Vector3 spawnPosition = new Vector3(Maths.GetSpawnPosition2D().X, 0.5f, Maths.GetSpawnPosition2D().Y);

            //Set object pos and set to active
            _obj.transform.position = spawnPosition;
            _obj.transform.rotation = _obj.transform.rotation;
            _obj.SetActive(true);

        }

        public void DespawnBird()
        {
            PoolingManager.Instance.CoolObject(_obj, poolingObjectType);
        }

        void IRoundObserver.OnNotify(RoundState state, int _currentRound, int _birdsNeeded, bool _isPerfectRound)
        {
            switch (state)
            {
                case RoundState.DUCKSPAWNING:
                    SpawnBird();
                    break;
            }

        }
    }
}