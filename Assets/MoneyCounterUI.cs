using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MoneyCounterUI : MonoBehaviour
{

    Text moneyAmount;

    void Awake()
    {
        moneyAmount = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyAmount.text = "MONEY: " + GameManager.money;
    }
}
