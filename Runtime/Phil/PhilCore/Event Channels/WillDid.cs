using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillDid {
	public EventChannel will = new EventChannel();
	public EventChannel did = new EventChannel();

	public void ClearAll(){
		will.ClearAll();
		did.ClearAll();
	}
}

public class WillDid<A> {
	public EventChannel<A> will = new EventChannel<A>();
	public EventChannel<A> did = new EventChannel<A>();

	public void ClearAll(){
		will.ClearAll();
		did.ClearAll();
	}
}

public class WillDid<A,B> {
	public EventChannel<A,B> will = new EventChannel<A, B>();
	public EventChannel<A,B> did = new EventChannel<A, B>();
	
	public void ClearAll(){
		will.ClearAll();
		did.ClearAll();
	}
}

public class WillDid<A,B,C> {
	public EventChannel<A,B,C> will = new EventChannel<A, B, C>();
	public EventChannel<A,B,C> did = new EventChannel<A, B, C>();

	public void ClearAll(){
		will.ClearAll();
		did.ClearAll();
	}
}