using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventHandlerPair<T> where T:Object {

	public T will;
	public T did;
}
