#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GoogleSheetLoader))]
public class GoogleSheetLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (GoogleSheetLoader)target;

        // 시트 오픈 버튼 추가
        if (GUILayout.Button("Open Sheet", GUILayout.Height(20)))
        {
            script.OpenGoogleSheet();
        }
        // 시트 다운로드 버튼 추가
        if (GUILayout.Button("Download Sheet", GUILayout.Height(20)))
        {
            script.DownloadGoogleSheet();
        }
    }
}
#endif