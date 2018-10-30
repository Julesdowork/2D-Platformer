using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    AudioManager audioManager;
    [SerializeField] string mouseHoverSoundName = "ButtonHover";
    [SerializeField] string buttonPressSoundName = "ButtonPress";

    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audio Manager found in scene.");
        }
    }

    public void Quit()
    {
        audioManager.PlaySound(buttonPressSoundName);
        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void Retry()
    {
        audioManager.PlaySound(buttonPressSoundName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSoundName);
    }
}
