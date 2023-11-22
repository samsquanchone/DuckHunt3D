using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(ResumeButtonPressed);
        mainMenuButton.onClick.AddListener(MainMenuButtonPressed);
        howToPlayButton.onClick.AddListener(HowToPlayButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
        
    }

    void ResumeButtonPressed()
    {
        GameManager.Instance.GameResumed();
        gameObject.SetActive(false);
    }

    void MainMenuButtonPressed()
    {
        SceneManager.Instance.TransationToScene(0);
    }

    void HowToPlayButtonPressed()
    {
        
    }

    void QuitButtonPressed()
    {
        Application.Quit();
    }
}
