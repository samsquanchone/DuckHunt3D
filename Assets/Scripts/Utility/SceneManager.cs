using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance => m_instance;
    private static SceneManager m_instance;

    private void Start()
    {
        m_instance = this;
    }
    public void TransationToScene(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
