using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField] string mainLevel;

	public void StartGame()
    {
        SceneManager.LoadScene(mainLevel);
    }

    public void QuitGame()
    {
        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }
}
