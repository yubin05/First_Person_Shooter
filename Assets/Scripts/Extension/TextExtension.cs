using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class TextExtension
{
#region TextInfoName
    // 텍스트 Name 데이터 담고 있는 컨테이너
    private static Dictionary<int, int> TextInfoNameIdDic = new Dictionary<int, int>();
    private static Dictionary<int, TextMeshProUGUI> TextInfoNameDic = new Dictionary<int, TextMeshProUGUI>();
    private static Dictionary<int, string[]> TextInfoNameReplaceDic = new Dictionary<int, string[]>();

    private static void AddTextInfoNameIdDic(this int instanceId, int id)
    {
        if (TextInfoNameIdDic.ContainsKey(instanceId)) return;
        TextInfoNameIdDic.Add(instanceId, id);
    }
    private static void AddTextInfoNameDic(this int instanceId, TextMeshProUGUI text)
    {
        if (TextInfoNameDic.ContainsKey(instanceId)) return;
        TextInfoNameDic.Add(instanceId, text);
    }
    private static void AddReplaceTextInfoNameDic(this int instanceId, params object[] replaces)
    {
        if (TextInfoNameReplaceDic.ContainsKey(instanceId)) TextInfoNameReplaceDic.Remove(instanceId);
        string[] strings = new string[replaces.Length];
        for (int i=0; i<strings.Length; i++) strings[i] = replaces[i].ToString();
        TextInfoNameReplaceDic.Add(instanceId, strings);
    }
    // 원하는 텍스트의 Name 데이터 불러오기
    public static void UpdateTextInfoName(this TextMeshProUGUI _text, int id, params object[] replaces)
    {
        var language = GameApplication.Instance.GameModel.ClientData.PlayerLanguage.language;
        int instanceId = _text.GetInstanceID();

        instanceId.AddTextInfoNameIdDic(id);
        instanceId.AddTextInfoNameDic(_text);
        instanceId.AddReplaceTextInfoNameDic(replaces);

        _text.text = ReplaceCSVText(id, TextInfo.Types.Name, language, replaces);
    }
#endregion
#region TextInfoDesc
    // 텍스트 Desc 데이터 담고 있는 컨테이너
    private static Dictionary<int, int> TextInfoDescIdDic = new Dictionary<int, int>();
    private static Dictionary<int, TextMeshProUGUI> TextInfoDescDic = new Dictionary<int, TextMeshProUGUI>();
    private static Dictionary<int, string[]> TextInfoDescReplaceDic = new Dictionary<int, string[]>();

    private static void AddTextInfoDescIdDic(this int instanceId, int id)
    {
        if (TextInfoDescIdDic.ContainsKey(instanceId)) return;
        TextInfoDescIdDic.Add(instanceId, id);
    }
    private static void AddTextInfoDescDic(this int instanceId, TextMeshProUGUI text)
    {
        if (TextInfoDescDic.ContainsKey(instanceId)) return;
        TextInfoDescDic.Add(instanceId, text);
    }
    private static void AddReplaceTextInfoDescDic(this int instanceId, params object[] replaces)
    {
        if (TextInfoDescReplaceDic.ContainsKey(instanceId)) TextInfoDescReplaceDic.Remove(instanceId);
        string[] strings = new string[replaces.Length];
        for (int i = 0; i < strings.Length; i++) strings[i] = replaces[i].ToString();
        TextInfoDescReplaceDic.Add(instanceId, strings);
    }

    private static void AddReplaceTextInfoDescDic(this int instanceId, string[] replaces)
    {
        if (TextInfoDescReplaceDic.ContainsKey(instanceId)) TextInfoDescReplaceDic.Remove(instanceId);
        TextInfoDescReplaceDic.Add(instanceId, replaces);
    }
    // 원하는 텍스트의 Desc 데이터 불러오기
    public static void UpdateTextInfoDesc(this TextMeshProUGUI _text, int id, params object[] replaces)
    {
        var language = GameApplication.Instance.GameModel.ClientData.PlayerLanguage.language;
        int instanceId = _text.GetInstanceID();

        instanceId.AddTextInfoDescIdDic(id);
        instanceId.AddTextInfoDescDic(_text);
        instanceId.AddReplaceTextInfoDescDic(replaces);

        _text.text = ReplaceCSVText(id, TextInfo.Types.Desc, language, replaces);
    }

    public static void UpdateTextInfoDesc(this TextMeshProUGUI _text, int id, string[] replaces)
    {
        var language = GameApplication.Instance.GameModel.ClientData.PlayerLanguage.language;
        int instanceId = _text.GetInstanceID();

        instanceId.AddTextInfoDescIdDic(id);
        instanceId.AddTextInfoDescDic(_text);
        instanceId.AddReplaceTextInfoDescDic(replaces);

        _text.text = ReplaceCSVText(id, TextInfo.Types.Desc, language, replaces);
    }

    #endregion
    #region Replace
    // 정해둔 규칙에 맞게 CSV 텍스트 데이터를 변환하는 함수
    // TextList의 @ -> ,로 변환
    // TextList의 [0] -> replaces의 0번 인덱스 참조하여 텍스트 덮어씌워짐
    // replaces의 "#{id}" -> 해당 id를 갖고있는 텍스트 리스트 데이터를 불러옴
    private static string ReplaceCSVText(int id, TextInfo.Types textType, TextInfo.LanguageTypes language, params object[] replaces)
    {
        var textInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<TextInfo>(nameof(TextInfo), id);
        string replaceCSVText = ReturnCSVText(id, textType, language);

        // replaceCSVText = replaceCSVText.Replace("_", " ");
        replaceCSVText = replaceCSVText.Replace("@", ",");

        for (int i=0; i<replaces.Length; i++)
        {
            var replace = replaces[i].ToString();
            if (replace.StartsWith("#"))
            {
                if (int.TryParse(replace.Trim('#'), out id))
                {
                    replace = ReturnCSVText(id, textType, language);
                }
            }
            replaceCSVText = replaceCSVText.Replace($"[{i}]", replace);
        }

        return replaceCSVText;
    }
    // 실제 TextList 참조해 텍스트 데이터 반환
    private static string ReturnCSVText(int id, TextInfo.Types textType, TextInfo.LanguageTypes language)
    {
        var textInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<TextInfo>(nameof(TextInfo), id);
        string returnCSVText = "";
        switch (textType)
        {
            case TextInfo.Types.Name:
            default:
            {
                switch (language)
                {
                    case TextInfo.LanguageTypes.Korean: default: returnCSVText = textInfo.NameKr; break;
                    case TextInfo.LanguageTypes.English: returnCSVText = textInfo.NameEn; break;
                }
                break;
            }
            case TextInfo.Types.Desc:
            {
                switch (language)
                {
                    case TextInfo.LanguageTypes.Korean: default: returnCSVText = textInfo.DescriptionKr; break;
                    case TextInfo.LanguageTypes.English: returnCSVText = textInfo.DescriptionEn; break;
                }
                break;
            }
        }
        return returnCSVText;
    }
#endregion
#region ChangeLanguage
    public static void ChangeLanauage(TextInfo.LanguageTypes language)
    {
        foreach (var text in TextInfoNameDic) text.Value.text = ReplaceCSVText(TextInfoNameIdDic[text.Key], TextInfo.Types.Name, language, TextInfoNameReplaceDic[text.Key]);
        foreach (var text in TextInfoDescDic) text.Value.text = ReplaceCSVText(TextInfoDescIdDic[text.Key], TextInfo.Types.Desc, language, TextInfoDescReplaceDic[text.Key]);
    }
#endregion
}
