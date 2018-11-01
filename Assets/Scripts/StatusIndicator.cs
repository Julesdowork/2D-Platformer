using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField] RectTransform healthBarFill;
    [SerializeField] Text healthText;

    Image image;

	// Use this for initialization
	void Start () {
		if (healthBarFill == null)
        {
            Debug.LogError("STATUS INDICATOR: No health bar object referenced.");
        }
        if (healthText == null)
        {
            Debug.LogError("STATUS INDICATOR: No health text object referenced.");
        }

        image = healthBarFill.GetComponent<Image>();
    }
	
	public void SetHealth(int _cur, int _max)
    {
        float _value = (float)_cur / _max;
        healthBarFill.localScale = new Vector3(_value, healthBarFill.localScale.y,
            healthBarFill.localScale.z);
        if (image != null)
            image.color = Color.Lerp(Color.red, Color.green, _value);
        healthText.text = _cur + "/" + _max + " HP";
    }
}
