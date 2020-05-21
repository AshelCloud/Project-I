using System.IO;
using System.Text;
using UnityEngine;

/*
                            Json 파일쓰고 읽기에 필요한 Util함수들
 */

public class JsonManager
{
    public static void ObjectToJsonWithCreate(string createPath, string fileName, object obj)
    {
        using (FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create))
        {
            var json = ObjectToJson(obj);

            byte[] data = Encoding.UTF8.GetBytes(json);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }
    }

    //isMapLoader 추가이유
    //앞 3바이트에 UTF-8 BOM이 추가됨. 테이블 Json이 읽히지않기 때문에 3바이트 제거
    //근데 맵 Json에는 추가가 되지않음 ( 아마 유니티에서 제작한 Json이기 때문인듯 )
    //그래서 구분해서 사용
    public static T LoadJson<T>(string loadPath, string fileName, bool isMapLoader = false)
    {
        using (FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open))
        {
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            
            string jsonData;
            if(isMapLoader)
            {
                jsonData = Encoding.UTF8.GetString(data, 0, data.Length);
            }
            else
            {
                jsonData = Encoding.UTF8.GetString(data, 3, data.Length - 3);
            }
            return JsonToObject<T>(jsonData);
        }
    }

    public static string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public static T JsonToObject<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }
}
