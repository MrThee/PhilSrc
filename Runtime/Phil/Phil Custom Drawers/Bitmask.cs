using UnityEngine;
using System.Collections;

namespace Phil {

namespace Attributes{

public class Bitmask : PropertyAttribute
{
    public System.Type propType;
    public Bitmask(System.Type aType)
    {
        propType = aType;
    }
}
}
}