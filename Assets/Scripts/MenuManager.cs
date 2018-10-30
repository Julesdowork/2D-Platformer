using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField] string mainLevel;
    AudioManager audioManager;
    [SerializeField] string hoverSound = "ButtonHover";
    [SerializeField] string pressSound = "ButtonPress";

    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager found!");
        }
    }

    public void StartGame()
    {
        audioManager.PlaySound(pressSound);
        SceneManager.LoadScene(mainLevel);
    }

    public void QuitGame()
    {
        audioManager.PlaySound(pressSound);
        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverSound);
    }
}
