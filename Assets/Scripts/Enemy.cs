using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        public int damage = 40;

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

    public EnemyStats stats = new EnemyStats();
    public GameObject deathFX;
    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;
    public string deathSoundName = "Explosion";
    public int moneyDrop = 10;

    [Header("Optional: ")]
    [SerializeField] StatusIndicator statusIndicator;

    void Start()
    {
        stats.Init();
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }

        GameManager.instance.OnToggleUpgradeMenu += OnUpgradeMenuToggle;

        if (deathFX == null)
        {
            Debug.LogError("No death particle effect referenced on Enemy.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.DamagePlayer(stats.damage);
            DamageEnemy(999);
        }
    }

    void OnDestroy()
    {
        GameManager.instance.OnToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }

    public void DamageEnemy(int damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            GameManager.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }

    void OnUpgradeMenuToggle(bool active)
    {
        // Handle what happens when Upgrade Menu is toggled
        GetComponent<EnemyAI>().enabled = !active;
    }
}
