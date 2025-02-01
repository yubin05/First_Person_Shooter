using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : View<MainMenuPresenter, MainMenuModel>
{
    [Header("버튼")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button quitBtn;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI playTxt;
    [SerializeField] private TextMeshProUGUI settingTxt;
    [SerializeField] private TextMeshProUGUI quitTxt;

    public override void UpdateUI(MainMenuModel model)
    {
        playTxt.UpdateTextInfoName(model.playTxtId);
        settingTxt.UpdateTextInfoName(model.settingTxtId);
        quitTxt.UpdateTextInfoName(model.quitTxtId);

        playBtn.onClick.RemoveAllListeners();
        playBtn.onClick.AddListener(() => 
        {
            var loadingCanvas = LoadingCanvas.Instance;
            loadingCanvas.StartCoroutine(loadingCanvas.LoadingPanel.LoadSceneAsync(nameof(BattleFieldScene)));
        });
    }
}

public class MainMenuPresenter : Presenter<MainMenuModel>
{
}

public class MainMenuModel : Model
{
    public int playTxtId = 1001;
    public int settingTxtId = 1002;
    public int quitTxtId = 1003;
}
