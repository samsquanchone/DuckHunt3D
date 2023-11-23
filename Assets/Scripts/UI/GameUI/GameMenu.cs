
using UnityEngine;
using UnityEngine.UI;
using Utility.SceneManagement;
using GamePlay.Manager;

namespace UI.GamePlay.Menu
{

    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;
        // Start is called before the first frame update
        void Start()
        {
            resumeButton.onClick.AddListener(ResumeButtonPressed);
            mainMenuButton.onClick.AddListener(MainMenuButtonPressed);
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

        void QuitButtonPressed()
        {
            Application.Quit();
        }
    }
}