using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Player player = null;
    public Image healthAmount = null;
    public Image staminaAmount = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void LateUpdate()
    {
        healthAmount.fillAmount = player.hp / player.max_HP;
    }
}
