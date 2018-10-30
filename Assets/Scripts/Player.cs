using System.Collections;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Player : MonoBehaviour {

    public int fallBoundary = -20;
    public string deathSoundName = "PlayerDeath";
    public string damageSoundName = "Grunt";

    [SerializeField] StatusIndicator statusIndicator;
    private AudioManager audioManager;
    private PlayerStats stats;

    void Start()
    {
        stats = PlayerStats.instance;
        stats.currentHealth = stats.maxHealth;

        if (statusIndicator == null)
        {
            Debug.Log("No status indicator referenced on Player.");
        }
        else
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }

        GameManager.instance.OnToggleUpgradeMenu += OnUpgradeMenuToggle;

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audio Manager found in scene.");
        }

        InvokeRepeating("RegenHealth", 1f / stats.healthRegenRate, 1f / stats.healthRegenRate);
    }

    void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(999);
        }
    }

    void OnDestroy()
    {
        GameManager.instance.OnToggleUpgradeMenu -= OnUpgradeMenuToggle;
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

    void OnUpgradeMenuToggle(bool active)
    {
        // Handle what happens when Upgrade Menu is toggled
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.enabled = !active;
        }
    }

    void RegenHealth()
    {
        stats.currentHealth += 1;
        statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
    }
}
