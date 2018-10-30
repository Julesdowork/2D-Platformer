using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

    [SerializeField] Text healthText;
    //[SerializeField] Text speedText;
    private PlayerStats stats;
    [SerializeField] float healthMult = 1.3f;
    //[SerializeField] float speedMult = 1.3f;
    [SerializeField] int upgradeCost = 50;

    void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "Health: " + stats.maxHealth;
        //speedText.text = stats.speed.ToString();
    }

    public void UpgradeHealth()
    {
        if (GameManager.money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        stats.maxHealth = Mathf.RoundToInt(stats.maxHealth * healthMult);
        GameManager.money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues();
    }

    //public void UpgradeSpeed()
    //{
    //    if (GameManager.money < upgradeCost)
    //    {
    //        AudioManager.instance.PlaySound("NoMoney");
    //        return;
    //    }
    //    stats.speed = Mathf.RoundToInt(stats.speed * speedMult);
    //    GameManager.money -= upgradeCost;
    //    AudioManager.instance.PlaySound("Money");
    //    UpdateValues();
    //}
}
