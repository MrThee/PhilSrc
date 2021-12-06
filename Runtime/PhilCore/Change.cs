using System.Collections.Generic;

namespace Phil.Core {

public struct Change<T> : System.IEquatable<Change<T>> 
{
    public bool doChange; // { get; private set; }

    public T value; // { get; private set; }

    public Change(T value, bool change){
        this.doChange = change;
        this.value = value;
    }

    public static Change<T> MaybeChange(T oldValue, T newValue){
        bool changed = ! EqualityComparer<T>.Default.Equals(oldValue, newValue);
        return new Change<T>(newValue, changed);
    }

    public bool TryApply(ref T dst){
        if(doChange){
            dst = value;
            return true;
        }
        return false;
    }

    public bool TryGet(out T newValue){
        if(doChange){
            newValue = value;
            return true;
        } else {
            newValue = default(T);
            return false;
        }
    }

    public void Append(Change<T> appendee){
        this.Append(ref appendee);
    }

    public void Append(ref Change<T> appendee){
        if(appendee.doChange){
            this.value = appendee.value;
            this.doChange = appendee.doChange;
        }
    }

    public bool To(out T newValue) => TryGet(out newValue);

    public static Change<T> Dont => new Change<T>(default(T), false);

    public static implicit operator Change<T>(T value){
        return new Change<T>(value, true);
    }

    public bool Equals(Change<T> other){
        return (this.doChange == other.doChange && EqualityComparer<T>.Default.Equals(this.value, other.value) );
    }
}

public static class Dont {

    public static Change<T> Change<T>() => new Change<T>(default(T), false);

}

}