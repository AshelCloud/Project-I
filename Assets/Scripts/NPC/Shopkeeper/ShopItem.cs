using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image icon;
    public Text descriptionText;
    public Text priceText;

    public ItemData Data { get; private set; }

    public void LinkingTextOfData(int code)
    {
        Data = ItemContainer.GetItem(code);

        icon.sprite = ResourcesContainer.Load<Sprite>("Sprites/Item/" + Data.GraphicAssetsName);
        descriptionText.text = Data.Name;
    }
}
