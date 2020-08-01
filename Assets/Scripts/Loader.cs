using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public GameObject bar;
    public Image barImage;
    public Text descriptionText;

    private IEnumerator Start()
    {
        descriptionText.text = "리소스 로딩중...";
        yield return null;

        ResourcesContainer.LoadAll("TileAssets");
        yield return null;

        descriptionText.text = "맵 데이터 로딩중...";
        yield return null;

        AssetBundle result = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
        if(result == null)
        {
            Log.PrintError("Failed to Load Jsons");
        }
        yield return null;

        descriptionText.text = "맵 타일 로딩중...";
        yield return null;

        barImage.fillAmount = 1f;
        descriptionText.text = "로딩 완료";

        yield return new WaitForSeconds(0.5f);

        Disable();
    }

    public void Enable()
    {
        //TODO: Disable랑 반대로
    }

    public void Disable()
    {
        GetComponent<Image>().enabled = false;
        bar.SetActive(false);
        barImage.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }
}
