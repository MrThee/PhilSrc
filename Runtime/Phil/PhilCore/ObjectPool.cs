using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Phil.Core {

public class ObjectPool<T> {

    private readonly Stack<T> m_pool;

    public ObjectPool(Func<T> DefaultConstructor,
        Action<T> OptWillUnPool, Action<T> OptWillRePool,
        int capacity
    ){
        this._DefaultConstructor = DefaultConstructor;
        this.mc_WillUnPool = OptWillUnPool;
        this.mc_WillRePool = OptWillRePool;
        this.m_pool = new Stack<T>(capacity);
        for(int i = 0; i < capacity; i++){
            this.RePool(this._DefaultConstructor());
        }
    }

    protected readonly Func<T> _DefaultConstructor;
    protected readonly Action<T> mc_WillUnPool;
    protected readonly Action<T> mc_WillRePool;

    public T UnPool(){
        if(m_pool.Count == 0){
            RePool(_DefaultConstructor());
        }
        T fromPool = m_pool.Pop();
        if(mc_WillUnPool != null){
            mc_WillUnPool(fromPool);
        }
        return fromPool;
    }

    public void RePool(T item){
        if(mc_WillRePool != null){
            mc_WillRePool(item);
        }
        m_pool.Push(item);
    }

}

public static class ObjectPool {
    public readonly static System.Action<UnityEngine.Component> SetActiveAfterUnPool;

    static ObjectPool(){
        SetActiveAfterUnPool = S_SetActiveAfterUnPool;
    }

    private static void S_SetActiveAfterUnPool(UnityEngine.Component componentHook){
        componentHook.gameObject.SetActive(true);
    }

    public static void SetInactiveBeforeRePool(UnityEngine.Component componentHook){
        componentHook.gameObject.SetActive(false);
    }

    public static void CommonWillUnPoolUI<S>(S item, Transform liveParent, 
        Func<S, RectTransform> GetRect, Action<RectTransform> SetRect) 
        where S:Component 
    {
        item.transform.SetParent(liveParent);
        SetRect?.Invoke(GetRect(item));
        item.gameObject.SetActive(true);
    }

    public static void CommonWillRePoolUI<S>(S item, Transform uiPoolParent) where S:Component {
        item.gameObject.SetActive(false);
        item.transform.SetParent(uiPoolParent);
    }
}

}