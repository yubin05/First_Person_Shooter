using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingPanel : View<LoadingPanelPresenter, LoadingPanelModel>
{
    [SerializeField] private TextMeshProUGUI loadingTxt;

    public override void UpdateUI(LoadingPanelModel model)
    {
        loadingTxt.UpdateTextInfoName(model.loadingTxtId);
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        OnShow();

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        yield return new WaitUntil(() => asyncOperation.progress <= 0.9f);
        asyncOperation.allowSceneActivation = true;

        Invoke(nameof(OnHide), 1f);
    }
}

public class LoadingPanelPresenter : Presenter<LoadingPanelModel>
{   
}

public class LoadingPanelModel : Model
{
    public int loadingTxtId = 1004;
}
