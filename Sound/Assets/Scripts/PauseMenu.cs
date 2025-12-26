using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Button returnButton;
    [SerializeField] private PlayerController player;

    private bool _isPaused;
    void Start()
    {
        returnButton.onClick.AddListener(Play);
        Time.timeScale = 1.0f;
    }

    public void Play()
    {
        ExitMenu();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
        {
            OpenMenu();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && _isPaused)
        {
            ExitMenu();
        }

    }
    private void OpenMenu()
    {
        _isPaused = true;
        menu.SetActive(true);
        player.allowed = false;
        Time.timeScale = 0.0f;
    }

    public void Reset()
    {
        Time.timeScale = 0.0f;
        SceneManager.LoadScene(0);
    }
    private void ExitMenu() 
    {
        _isPaused = false;
        menu.SetActive(false);
        player.allowed = true;
        Time.timeScale = 1.0f;
    }
    public void StartTime()
    {
        Time.timeScale = 1.0f;
    }
}
