using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public Image barImage;
    public Text descriptionText;

    private IEnumerator Start()
    {
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

        result = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "tiles"));
        if(result == null)
        {
            Log.PrintError("Failed to Load Tiles");
        }

        yield return null;

        descriptionText.text = "로딩 완료";

        yield return new WaitForSeconds(0.5f);

        //TODO: Scene 이동 바꾸기
        SceneManager.LoadSceneAsync("ProtoType");
    }
}
