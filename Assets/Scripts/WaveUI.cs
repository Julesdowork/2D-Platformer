using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

    [SerializeField] WaveSpawner spawner;
    [SerializeField] Animator waveAnimator;
    [SerializeField] Text waveCountdownText;
    [SerializeField] Text waveNumberText;

    WaveSpawner.SpawnState prevState;

	// Use this for initialization
	void Start () {
		if (spawner == null)
        {
            Debug.LogError("No WaveSpawner referenced.");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No WaveSpawner referenced.");
            this.enabled = false;
        }
        if (waveCountdownText == null)
        {
            Debug.LogError("No WaveSpawner referenced.");
            this.enabled = false;
        }
        if (waveNumberText == null)
        {
            Debug.LogError("No WaveSpawner referenced.");
            this.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		switch (spawner.State)
        {
            case WaveSpawner.SpawnState.Counting:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.Spawning:
                UpdateSpawningUI();
                break;
        }

        prevState = spawner.State;
	}

    void UpdateCountingUI()
    {
        if (prevState != WaveSpawner.SpawnState.Counting)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
            //Debug.Log("Counting down...");
        }
        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
    }

    void UpdateSpawningUI()
    {
        if (prevState != WaveSpawner.SpawnState.Spawning)
        {
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);
            waveNumberText.text = spawner.NextWave.ToString();
            //Debug.Log("Spawning...");
        }
    }
}
