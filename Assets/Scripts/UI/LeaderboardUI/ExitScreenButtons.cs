using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.SceneManagement;

namespace UI.LeaderBoards.Navigation
{
    public class ExitScreenButtons : MonoBehaviour
    {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;
        void Start()
        {
            mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
            quitButton.onClick.AddListener(QuitButtonClicked);
        }

        void MainMenuButtonClicked()
        {
            SceneManager.Instance.TransationToScene(0);
        }

        void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}