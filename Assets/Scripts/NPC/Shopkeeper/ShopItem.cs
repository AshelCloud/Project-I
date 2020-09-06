using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image icon;
    public Text descriptionText;
    public Text priceText;

    public ItemData Data { get; private set; }

    public int Code { get; private set; }

    public void LinkingTextOfData(int code)
    {
        Code = code;

        Data = ItemContainer.GetItem(code);

        icon.sprite = ResourcesContainer.Load<Sprite>("Sprites/" + Data.Route);
        descriptionText.text = Data.Name;
        priceText.text = Data.Cost.ToString() + "Gold";
    }
}
