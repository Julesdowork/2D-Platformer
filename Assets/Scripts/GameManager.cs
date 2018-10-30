using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 3.5f;
    public Transform spawnPrefab;
    public CameraShake cameraShake;

    static int _remainingLives = 3;

    AudioSource audioSource;
    [SerializeField] GameObject gameOverUI;

    void Awake()
    {
        if (instance == null)
            instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("No CameraShake referenced in Game Manager.");
        }
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        _remainingLives--;
        if (_remainingLives <= 0)
        {
            instance.EndGame();
        }
        else
        {
            instance.StartCoroutine(instance.RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        instance._KillEnemy(enemy);
    }

    public void EndGame()
    {
        Debug.Log("GAME OVER!");
        gameOverUI.SetActive(true);
    }

    public IEnumerator RespawnPlayer()
    {
        audioSource.Play();
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, Quaternion.identity);
        Destroy(clone.gameObject, 3f);
    }

    public void _KillEnemy(Enemy enemy)
    {
        GameObject clone = Instantiate(enemy.deathFX, enemy.transform.position, Quaternion.identity);
        Destroy(clone, 3f);
        cameraShake.Shake(enemy.shakeAmount, enemy.shakeLength);
        Destroy(enemy.gameObject);
    }
}
