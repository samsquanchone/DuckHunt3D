using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance => m_instance;
    private static SpawnManager m_instance;
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
        yield return new WaitForSeconds(2f);

        //Get object from pool
        GameObject _obj = PoolingManager.Instance.GetPoolObject(PoolingObjectType.DUCK);
        
        //Calculate spawn position
        Vector3 spawnPosition = new Vector3(Maths.GetSpawnPosition2D().X, 0.5f, Maths.GetSpawnPosition2D().Y);

        //Set object pos and set to active
        _obj.transform.position = spawnPosition;
        _obj.transform.rotation = _obj.transform.rotation;
        _obj.SetActive(true);

    }
}
