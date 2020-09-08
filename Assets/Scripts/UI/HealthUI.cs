using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private UnityEngine.UI.Image healthAmount = null;

    private void Awake()
    {
        healthAmount = transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
    }

    private void LateUpdate()
    {
        healthAmount.fillAmount = Player.hp / 100;
    }
}
