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

    private float CurrentProgressBarValue { get; set; }

    private int LoadCount { get; set; }

    private int CurrentLoadCount { get; set; }

    public bool Loaded { get; set; }
    private bool IsInGame { get; set; }

    public float FadedValue
    {
        get
        {
            return backgroundImage.color.a;
        }
    }

    Coroutine CurrentCoroutine { get; set; }

    public void ActiveLoading()
    {
        Loaded = false;
        IsInGame = false;

        CurrentProgressBarValue = 0f;
        LoadCount = 2;
        CurrentLoadCount = 0;

        StartCoroutine(FillProgressBar());
        CurrentCoroutine = StartCoroutine(Loading());
    }

    public void ActiveFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void ActiveFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator Loading()
    {
        descriptionText.text = "맵 데이터 로딩중...";
        yield return null;
        LoadMapDatas();

        descriptionText.text = "맵 타일 로딩중...";
        yield return null;
        LoadTileResources();
        
        descriptionText.text = "로딩 완료";

        yield return new WaitForSeconds(0.5f);

        Loaded = true;
        CurrentCoroutine = null;
        yield return null;
    }

    private void LoadTileResources()
    {
        ResourcesContainer.LoadAll("TileAssets");

        CurrentLoadCount ++;
    }

    private void LoadMapDatas()
    {
        AssetBundle result = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
        if (result == null)
        {
            Log.PrintError("Failed to Load Jsons");
        }

        CurrentLoadCount++;
    }

    public void Enable()
    {
        bar.SetActive(true);
        barImage.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);
    }

    public void Disable()
    {
        bar.SetActive(false);
        barImage.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }

    public IEnumerator FillProgressBar()
    {
        while(CurrentProgressBarValue < 1f)
        {
            CurrentProgressBarValue = (float)CurrentLoadCount / LoadCount;

            barImage.fillAmount = CurrentProgressBarValue;
            yield return null;
        }

        yield return null;
    }
    public IEnumerator FadeOut()
    {
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

    public IEnumerator FadeIn()
    {
        backgroundImage.enabled = true;
        yield return null;

        while (backgroundImage.color.a < 1f)
        {
            Color co = backgroundImage.color;
            backgroundImage.color = new Color(co.r, co.g, co.b, co.a + fadeSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
