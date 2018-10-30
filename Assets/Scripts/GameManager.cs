using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }
    public static int money;

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 3.5f;
    public Transform spawnPrefab;
    public CameraShake cameraShake;
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback OnToggleUpgradeMenu;

    static int _remainingLives;
    
    [SerializeField] GameObject gameOverUI;
    [SerializeField] int maxLives = 3;
    AudioManager audioManager;
    [SerializeField] GameObject upgradeMenu;
    [SerializeField] int startingMoney;
    [SerializeField] WaveSpawner waveSpawner;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("No CameraShake referenced in Game Manager.");
        }

        _remainingLives = maxLives;
        money = startingMoney;

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager found in the scene.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
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
        audioManager.PlaySound(gameOverSoundName);
        Debug.Log("GAME OVER!");
        gameOverUI.SetActive(true);
    }

    public IEnumerator RespawnPlayer()
    {
        audioManager.PlaySound(respawnCountdownSoundName);
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound(spawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, Quaternion.identity);
        Destroy(clone.gameObject, 3f);
    }

    public void _KillEnemy(Enemy enemy)
    {
        // Let's play a sound
        audioManager.PlaySound(enemy.deathSoundName);

        // Gain some coinage
        money += enemy.moneyDrop;
        audioManager.PlaySound("Money");

        // Add particles
        GameObject clone = Instantiate(enemy.deathFX, enemy.transform.position, Quaternion.identity);
        Destroy(clone, 3f);

        // Do camera shake
        cameraShake.Shake(enemy.shakeAmount, enemy.shakeLength);
        Destroy(enemy.gameObject);
    }

    void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        OnToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }
}
