using System;

namespace Phil.Core {

public struct StackArray16<T>
{
    public int length => 16;

    public static StackArray16<T> AllTheSame(T sameInitalValue){
        var arr = new StackArray16<T>();
        for(int i = 0; i < 16; i++){
            arr[i] = sameInitalValue;
        }
        return arr;
    }

    private ((T,T,T,T), (T,T,T,T), (T,T,T,T), (T,T,T,T)) m_arr;

    public T this[int i]{
        get {
            switch(i){
            default: throw new ArgumentException("Index out of range", nameof(i));
            case 0: return m_arr.Item1.Item1;
            case 1: return m_arr.Item1.Item2;
            case 2: return m_arr.Item1.Item3;
            case 3: return m_arr.Item1.Item4;
            case 4: return m_arr.Item2.Item1;
            case 5: return m_arr.Item2.Item2;
            case 6: return m_arr.Item2.Item3;
            case 7: return m_arr.Item2.Item4;
            case 8: return m_arr.Item3.Item1;
            case 9: return m_arr.Item3.Item2;
            case 10: return m_arr.Item3.Item3;
            case 11: return m_arr.Item3.Item4;
            case 12: return m_arr.Item4.Item1;
            case 13: return m_arr.Item4.Item2;
            case 14: return m_arr.Item4.Item3;
            case 15: return m_arr.Item4.Item4;
            }
        }

        set {
            switch(i){
            default: throw new ArgumentException("Index out of range", nameof(i));
            case 0: m_arr.Item1.Item1 = value; break;
            case 1: m_arr.Item1.Item2 = value; break;
            case 2: m_arr.Item1.Item3 = value; break;
            case 3: m_arr.Item1.Item4 = value; break;
            case 4: m_arr.Item2.Item1 = value; break;
            case 5: m_arr.Item2.Item2 = value; break;
            case 6: m_arr.Item2.Item3 = value; break;
            case 7: m_arr.Item2.Item4 = value; break;
            case 8: m_arr.Item3.Item1 = value; break;
            case 9: m_arr.Item3.Item2 = value; break;
            case 10: m_arr.Item3.Item3 = value; break;
            case 11: m_arr.Item3.Item4 = value; break;
            case 12: m_arr.Item4.Item1 = value; break;
            case 13: m_arr.Item4.Item2 = value; break;
            case 14: m_arr.Item4.Item3 = value; break;
            case 15: m_arr.Item4.Item4 = value; break;
            }
        }
    }
}

}

