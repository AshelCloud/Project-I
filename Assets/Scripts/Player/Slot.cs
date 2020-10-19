using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public void SetField(Sprite icon, string slotName)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = icon;
        transform.GetChild(1).GetComponent<Text>().text = slotName;
    }
}
