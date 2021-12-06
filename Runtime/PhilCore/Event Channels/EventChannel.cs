using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChannel {

	// public delegate void VoidDel();
	private event System.Action mk_channel;

	public EventChannel(){
	}

	public void Add(System.Action function){
		this.mk_channel += function;
	}

	public void Remove(System.Action function){
		this.mk_channel -= function;
	}

	public void Invoke(){
		if (mk_channel != null) {
			mk_channel.Invoke ();
		}
	}

	public void ClearAll(){
		mk_channel = null;
	}
		
}
