using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A stands for Arguement Type
public class EventChannel<A> {

	private EventChannel mk_0AChannel = new EventChannel();

	private event System.Action<A> mk_channel;

	public EventChannel() : base() {}

	public void Add(System.Action<A> callback){
		this.mk_channel += callback;
	}

	public void Remove(System.Action<A> callback){
		this.mk_channel -= callback;
	}

	// Glue
	public void Add(System.Action voidCB){
		this.mk_0AChannel.Add (voidCB);
	}

	public void Remove(System.Action voidCB){
		this.mk_0AChannel.Remove (voidCB);
	}

	public void Invoke(A arg){
		if (mk_channel != null) {
			mk_channel.Invoke (arg);
		}
		mk_0AChannel.Invoke ();
	}

	public void ClearAll(){
		mk_0AChannel.ClearAll();
		mk_channel = null;
	}
}

public class EventChannel<A,B> {

	private event System.Action mk_0Channel;
	private event System.Action<A> mk_AChannel;
	private event System.Action<B> mk_BChannel;

	private event System.Action<A,B> mk_channel;

	public EventChannel() : base() {}

	public void Add(System.Action<A,B> callback){
		this.mk_channel += callback;
	}

	public void Remove(System.Action<A,B> callback){
		this.mk_channel -= callback;
	}

	public void Invoke(A arg1, B arg2){
		if (mk_channel != null) {
			mk_channel.Invoke (arg1, arg2);
		}
		if (mk_AChannel != null) {
			mk_AChannel.Invoke (arg1);
		}
		if (mk_BChannel != null) {
			mk_BChannel.Invoke (arg2);
		}
		if (mk_0Channel != null) {
			mk_0Channel.Invoke ();
		}
	}

	public void Add(System.Action<A> callback){
		this.mk_AChannel += callback;
	}

	public void Add(System.Action<B> callback){
		this.mk_BChannel += callback;
	}

	public void Add(System.Action callback){
		this.mk_0Channel += callback;
	}

	public void Remove(System.Action<A> callback){
		this.mk_AChannel -= callback;
	}

	public void Remove(System.Action<B> callback){
		this.mk_BChannel -= callback;
	}

	public void Remove(System.Action callback){
		this.mk_0Channel -= callback;
	}

	public void ClearAll(){
		this.mk_0Channel = null;
		this.mk_AChannel = null;
		this.mk_BChannel = null;
		this.mk_channel = null;
	}
}

public class EventChannel<A,B,C> {

	private event System.Action mk_0Channel;
	private event System.Action<A> mk_AChannel;
	private event System.Action<B> mk_BChannel;
	private event System.Action<C> mk_CChannel;
	private event System.Action<A,B> mk_ABChannel;
	private event System.Action<B,C> mk_BCChannel;
	private event System.Action<A,C> mk_ACChannel;

	private event System.Action<A,B,C> mk_channel;

	public EventChannel(){}

	public void Add(System.Action<A,B,C> callback){
		this.mk_channel += callback;
	}

	public void Remove(System.Action<A,B,C> callback){
		this.mk_channel -= callback;
	}

	public void Invoke(A arg1, B arg2, C arg3){
		if (mk_channel != null) {
			mk_channel.Invoke (arg1, arg2, arg3);
		}
		if (mk_ABChannel != null) {
			mk_ABChannel.Invoke (arg1, arg2);
		}
		if (mk_BCChannel != null) {
			mk_BCChannel.Invoke (arg2, arg3);
		}
		if (mk_ACChannel != null) {
			mk_ACChannel.Invoke (arg1, arg3);
		}
		if (mk_AChannel != null) {
			mk_AChannel.Invoke (arg1);
		}
		if (mk_BChannel != null) {
			mk_BChannel.Invoke (arg2);
		}
		if (mk_CChannel != null) {
			mk_CChannel.Invoke (arg3);
		}
		if (mk_0Channel != null) {
			mk_0Channel.Invoke ();
		}
	}

	public void Add(System.Action<A,B> callback){
		this.mk_ABChannel += callback;
	}

	public void Add(System.Action<B,C> callback){
		this.mk_BCChannel += callback;
	}

	public void Add(System.Action<A,C> callback){
		this.mk_ACChannel += callback;
	}

	public void Add(System.Action<A> callback){
		this.mk_AChannel += callback;
	}

	public void Add(System.Action<B> callback){
		this.mk_BChannel += callback;
	}

	public void Add(System.Action<C> callback){
		this.mk_CChannel += callback;
	}

	public void Add(System.Action callback){
		this.mk_0Channel += callback;
	}

	public void Remove(System.Action<A,B> callback){
		this.mk_ABChannel -= callback;
	}

	public void Remove(System.Action<B,C> callback){
		this.mk_BCChannel -= callback;
	}

	public void Remove(System.Action<A,C> callback){
		this.mk_ACChannel -= callback;
	}

	public void Remove(System.Action<A> callback){
		this.mk_AChannel -= callback;
	}

	public void Remove(System.Action<B> callback){
		this.mk_BChannel -= callback;
	}

	public void Remove(System.Action<C> callback){
		this.mk_CChannel -= callback;
	}

	public void Remove(System.Action callback){
		this.mk_0Channel -= callback;
	}

	public void ClearAll(){
		mk_0Channel = null;

		mk_AChannel = null;
		mk_BChannel = null;
		mk_CChannel = null;
		
		mk_ABChannel = null;
		mk_BCChannel = null;
		mk_ACChannel = null;
		
		mk_channel = null;
	}
}
