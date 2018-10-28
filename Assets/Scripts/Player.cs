using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]
	public class PlayerStats
    {
        public int health = 100;
    }

    public PlayerStats playerStats = new PlayerStats();
    public int fallBoundary = -20;

    void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(999);
        }
    }

    public void DamagePlayer(int damage)
    {
        playerStats.health -= damage;
        if (playerStats.health <= 0)
        {
            GameManager.KillPlayer(this);
        }
    }
}
