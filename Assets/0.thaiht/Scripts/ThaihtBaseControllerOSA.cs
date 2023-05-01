using System;
using UnityEngine;
using Com.TheFallenGames.OSA.Core;
using UnityEngine.UI;

public abstract class ThaihtBaseControllerOSA<TAdapter, TParams, TViewsHolder> : MonoBehaviour
    where TAdapter : OSA<TParams, TViewsHolder>
    where TParams : BaseParams
    where TViewsHolder : BaseItemViewsHolder
{
    protected TAdapter[] _Adapters;




    int _InitializedAdapters;


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        InitAdapters();


        _InitializedAdapters = 0;
        foreach (var adapter in _Adapters)
        {
            var copyOfAdapter = adapter;
            if (copyOfAdapter.IsInitialized)
                ++_InitializedAdapters;
            else
            {
                Action onInitializedWithAutoUnsubscribe = null;
                onInitializedWithAutoUnsubscribe = () =>
                {
                    copyOfAdapter.Initialized -= onInitializedWithAutoUnsubscribe;
                    OnAdapterInitialized();
                };
                copyOfAdapter.Initialized += onInitializedWithAutoUnsubscribe;
            }
        }

        // If not all initialized, OnAllAdaptersInitialized will be called when OnAdapterInitialized will be called for last one 
        if (_InitializedAdapters == _Adapters.Length)
            OnAllAdaptersInitialized();
    }

    protected virtual void Update() { }

    protected virtual void InitAdapters()
    {
        _Adapters = FindObjectsOfType<TAdapter>();
    }

    protected virtual void OnAdapterInitialized()
    {
        if (++_InitializedAdapters == _Adapters.Length)
            OnAllAdaptersInitialized();
    }

    protected virtual void OnAllAdaptersInitialized()
    {

    }

    #region events from DrawerCommandPanel
    void OnAddItemRequested(int index)
    {
        foreach (var adapter in _Adapters)
            OnAddItemRequested(adapter, index);
    }

    void OnRemoveItemRequested(int index)
    {
        foreach (var adapter in _Adapters)
            OnRemoveItemRequested(adapter, index);
    }

    void OnItemCountChangeRequested(int count)
    {
        foreach (var adapter in _Adapters)
            OnItemCountChangeRequested(adapter, count);
    }

    protected virtual void OnAddItemRequested(TAdapter adapter, int index)
    {

    }

    protected virtual void OnRemoveItemRequested(TAdapter adapter, int index)
    {

    }

    protected virtual void OnItemCountChangeRequested(TAdapter adapter, int count)
    {

    }
    #endregion
}

