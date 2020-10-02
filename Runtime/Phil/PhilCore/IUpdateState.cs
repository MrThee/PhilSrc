using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateState<T>
{
    void UpdateState(T delta);
}
