using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance => m_instance;
    private static SpawnManager m_instance;

    GameObject _obj;

    [SerializeField] UnityEvent despawnBird;
  
    // Start is called before the first frame update
    void Awake()
    {
        m_instance = this;
    }

    public void SpawnBird()
    {
        StartCoroutine("WaitToSpawn");
    }
    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(1.5f);

        //Get object from pool
        _obj = PoolingManager.Instance.GetPoolObject(PoolingObjectType.DUCK);
        
        //Calculate spawn position
        Vector3 spawnPosition = new Vector3(Maths.GetSpawnPosition2D().X, 0.5f, Maths.GetSpawnPosition2D().Y);

        //Set object pos and set to active
        _obj.transform.position = spawnPosition;
        _obj.transform.rotation = _obj.transform.rotation;
        _obj.SetActive(true);

    }

    public void DespawnBird()
    {
        PoolingManager.Instance.CoolObject(_obj, PoolingObjectType.DUCK);
        despawnBird.Invoke();
    }

    

}