using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Phil;
using Phil.Core;

namespace Phil.Core {

[System.Serializable]
public class PolyObject<T,A,B> where T:System.Enum 
    where A:UnityEngine.Object where B:UnityEngine.Object
{

    public T type;
    public A componentA;
    public B componentB;

}

[System.Serializable]
public class PolyObject<T,A,B,C> where T:System.Enum 
    where A:UnityEngine.Object where B:UnityEngine.Object where C:UnityEngine.Object
{

    public T type;
    public A componentA;
    public B componentB;
    public C componentC;

}

[System.Serializable]
public class PolyObject<T,A,B,C,D> where T:System.Enum
    where A:UnityEngine.Object where B:UnityEngine.Object where C:UnityEngine.Object where D:UnityEngine.Object
{

    public T type;
    public A componentA;
    public B componentB;
    public C componentC;
    public D componentD;

}

}