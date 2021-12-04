using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillDid {
	public EventChannel Will = new EventChannel();
	public EventChannel Did = new EventChannel();

	public void ClearAll(){
		Will.ClearAll();
		Did.ClearAll();
	}
}

public class WillDid<A> {
	public EventChannel<A> Will = new EventChannel<A>();
	public EventChannel<A> Did = new EventChannel<A>();

	public void ClearAll(){
		Will.ClearAll();
		Did.ClearAll();
	}
}

public class WillDid<A,B> {
	public EventChannel<A,B> Will = new EventChannel<A, B>();
	public EventChannel<A,B> Did = new EventChannel<A, B>();
	
	public void ClearAll(){
		Will.ClearAll();
		Did.ClearAll();
	}
}

public class WillDid<A,B,C> {
	public EventChannel<A,B,C> Will = new EventChannel<A, B, C>();
	public EventChannel<A,B,C> Did = new EventChannel<A, B, C>();

	public void ClearAll(){
		Will.ClearAll();
		Did.ClearAll();
	}
}