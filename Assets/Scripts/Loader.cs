using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [Header("Settings")]
    public float fadeSpeed = 1f;

    [Header("Components")]
    public Image backgroundImage;
    public GameObject bar;
    public Image barImage;
    public Text descriptionText;

    public bool Loaded { get; set; }
    private bool IsInGame { get; set; }

    Coroutine CurrentCoroutine { get; set; }

    public void ActiveLoading()
    {
        Loaded = false;
        IsInGame = false;

        CurrentCoroutine = StartCoroutine(Loading());
    }

    public void ActiveFadeOut()
    {
        if(Loaded && IsInGame == false)
        {
            if(CurrentCoroutine == null)
            {
                Log.Print("Run Coroutine");
                CurrentCoroutine = StartCoroutine(FadeOut());
            }
        }
    }

    private IEnumerator Loading()
    {
        descriptionText.text = "리소스 로딩중...";
        yield return null;

        ResourcesContainer.LoadAll("TileAssets");
        yield return null;

        descriptionText.text = "맵 데이터 로딩중...";
        yield return null;

        AssetBundle result = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
        if (result == null)
        {
            Log.PrintError("Failed to Load Jsons");
        }
        yield return null;

        descriptionText.text = "맵 타일 로딩중...";
        yield return null;

        barImage.fillAmount = 1f;
        descriptionText.text = "로딩 완료";

        yield return new WaitForSeconds(0.5f);

        Loaded = true;
        CurrentCoroutine = null;
    }

    public void Enable()
    {
        backgroundImage.enabled = true;
        bar.SetActive(true);
        barImage.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);
    }

    //public void Disable()
    //{
    //    backgroundImage.enabled = false;
    //    bar.SetActive(false);
    //    barImage.gameObject.SetActive(false);
    //    descriptionText.gameObject.SetActive(false);
    //}

    public IEnumerator FadeOut()
    {
        bar.SetActive(false);
        barImage.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
        yield return null;

        while(backgroundImage.color.a > 0f)
        {
            Color co = backgroundImage.color;
            backgroundImage.color = new Color(co.r, co.g, co.b, co.a - fadeSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        backgroundImage.enabled = false;
        IsInGame = true;

        yield return null;
    }
}
