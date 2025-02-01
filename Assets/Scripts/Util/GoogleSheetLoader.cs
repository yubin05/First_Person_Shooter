#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "List", menuName = "Create TableList", order = int.MaxValue)]
public class GoogleSheetLoader : ScriptableObject   // 구글 시트 로더하는 스크립트오브젝트
{
    public string sheetId;
    public string gid;
    public string sheetName;

    // 시트 오픈
    public void OpenGoogleSheet()
    {
        // 시트 오픈 URL
        var sheetUrl = string.Format("https://docs.google.com/spreadsheets/d/{0}/edit?gid={1}#gid={1}", sheetId, gid);
        Application.OpenURL(sheetUrl);
    }

    // 시트 다운로드
    public void DownloadGoogleSheet()
    {
        // 시트 다운로드 URL
        var downloadUrl = string.Format("https://docs.google.com/spreadsheets/d/{0}/export?format=csv&id={0}&gid={1}", sheetId, gid);

        using (var www = UnityWebRequest.Get(downloadUrl))
        {
            var async = www.SendWebRequest();

            var timeout = 300;
            while (!async.isDone)
            {
                System.Threading.Thread.Sleep(100); // 0.1초 대기
                if (--timeout < 0) break;
            }

            if (async.isDone)
            {
                if (www.result != UnityWebRequest.Result.ConnectionError)
                {
                    var text = www.downloadHandler.text;

                    // 시트 저장 경로
                    var path = string.Format("Assets/Resources/{0}{1}.csv", DataTablePath.CSVFilePath, sheetName);
                    File.WriteAllText(path, text);

                    AssetDatabase.Refresh();
                }
            }
        }
    }
}
#endif