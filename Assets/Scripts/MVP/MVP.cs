using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView<M>
{
    public void UpdateUI(M model);
}
/// <summary>
/// View -> 화면에 보여줄 UI 요소들, Presenter를 통해 데이터를 전달받음
/// </summary>
/// <typeparam name="M"></typeparam>
public abstract class View<P, M> : MonoBehaviour, IView<M> where P : Presenter<M>, new() where M : Model, new()
{
    protected P presenter;

    public virtual void Init()
    {
        presenter = new P();
        presenter.Init(this);

        OnShow();
    }

    public virtual void UpdateUI()
    {
        if (presenter == null) return;
        presenter.UpdateUI();
    }

    public abstract void UpdateUI(M model);

    public virtual void OnShow()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnHide()
    {
        gameObject.SetActive(false);
    }
}

/// <summary>
/// Presenter -> Model에 데이터를 담아 View로 전달해주는 매개체
/// </summary>
/// <typeparam name="M"></typeparam>
public abstract class Presenter<M> where M : Model, new()
{
    protected IView<M> view;
    protected M model;

    public void Init(IView<M> view)
    {
        this.view = view;
        model = new M();

        UpdateUI();
    }

    /// <summary>
    /// 상속 받는 클래스에서 base.UpdateUI()를 마지막에 호출해야 함
    /// </summary>
    public virtual void UpdateUI()
    {
        view.UpdateUI(model);
    }
}

/// <summary>
/// Model -> View에 뿌려줄 데이터를 갖고있는 클래스 (Presenter를 통해 전달)
/// </summary>
public abstract class Model
{
}
