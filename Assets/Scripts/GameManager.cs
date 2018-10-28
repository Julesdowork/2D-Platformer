using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        instance.StartCoroutine(instance.RespawnPlayer());
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        // TODO add spawn particles
    }
}
