using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthAmount = null;
    public Image staminaAmount = null;

    private void LateUpdate()
    {
        healthAmount.fillAmount = Player.hp / 100;
    }
}
