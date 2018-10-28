using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
    }
}
