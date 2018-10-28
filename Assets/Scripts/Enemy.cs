using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int health = 100;
    }

    public EnemyStats stats = new EnemyStats();

    public void DamageEnemy(int damage)
    {
        stats.health -= damage;
        if (stats.health <= 0)
        {
            GameManager.KillEnemy(this);
        }
    }
}
