using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class ComponentManagerBase : MonoBehaviour
{
    #region ActionAndFuncInvokeHelper
    public void InvokeAction(Action _action)
    {
        _action?.Invoke();
    }
    public void InvokeAction<T>(Action<T> _action, T _arg)
    {
        _action?.Invoke(_arg);
    }
    public void InvokeAction<T1, T2>(Action<T1, T2> _action, T1 _arg1, T2 _arg2)
    {
        _action?.Invoke(_arg1, _arg2);
    }
    public void InvokeAction<T1, T2, T3>(Action<T1, T2, T3> _action, T1 _arg1, T2 _arg2, T3 _arg3)
    {
        _action?.Invoke(_arg1, _arg2, _arg3);
    }
    public T InvokeFunc<T>(Func<T> _func)
    {
        Assert.IsNotNull(_func, GetType().Name + "的服务缺少提供者");
        Assert.IsTrue(_func.GetInvocationList().Length == 1, "服务" + _func.ToString() + "有复数提供者");
        return _func.Invoke();
    }
    public T2 InvokeFunc<T1, T2>(Func<T1, T2> _func, T1 _arg)
    {
        Assert.IsNotNull(_func, GetType().Name + "的服务缺少提供者");
        Assert.IsTrue(_func.GetInvocationList().Length == 1, "服务" + _func.ToString() + "有复数提供者");
        return _func.Invoke(_arg);
    }
    public T3 InvokeFunc<T1, T2, T3>(Func<T1, T2, T3> _func, T1 _arg0, T2 _arg1)
    {
        Assert.IsNotNull(_func, GetType().Name + "的服务缺少提供者");
        Assert.IsTrue(_func.GetInvocationList().Length == 1, "服务" + _func.ToString() + "有复数提供者");
        return _func.Invoke(_arg0, _arg1);
    }
    #endregion
}
