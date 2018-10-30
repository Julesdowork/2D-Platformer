using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]
	public class PlayerStats
    {
        public int maxHealth = 100;

        private int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public PlayerStats stats = new PlayerStats();
    public int fallBoundary = -20;
    public string deathSoundName = "PlayerDeath";
    public string damageSoundName = "Grunt";

    [SerializeField] private StatusIndicator statusIndicator;
    AudioManager audioManager;

    void Start()
    {
        stats.Init();

        if (statusIndicator == null)
        {
            Debug.Log("No status indicator referenced on Player.");
        }
        else
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audio Manager found in scene.");
        }
    }

    void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(999);
        }
    }

    public void DamagePlayer(int damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            // Play death sound
            audioManager.PlaySound(deathSoundName);

            // Kill player
            GameManager.KillPlayer(this);
        }
        else
        {
            // Play damage sound
            audioManager.PlaySound(damageSoundName);
        }


        if (statusIndicator == null)
            Debug.Log("No status indicator referenced on Player.");
        else
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
    }
}
