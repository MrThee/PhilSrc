using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

[System.Serializable]
public class Grid2D<T> {

    public int width { get; private set; }
    public int height { get; private set; }
    public int area => width*height;

    private List<T> m_values;

    public Grid2D(int width, int height, T defaultValue){
        this.width = width;
        this.height = height;
        this.m_values = new List<T>(width*height);
        int a = area;
        for(int i = 0; i < area; i++){
            m_values.Add(defaultValue);
        }
    }

    public void Resize(int newWidth, int newHeight){
        this.width = newWidth;
        this.height = newHeight;
    }

    public void SetAll(T value){
        for(int i = 0; i < area; i++){
            m_values[i] = value;
        }
    }

    public int2 GetCoord(int flatIndex){
        return new int2(flatIndex % width, flatIndex / width);
    }

    private int GetFlatIndex(int x, int y){
        return width*y + x;
    }

    public T this[int x, int y]{
        get {
            return m_values[GetFlatIndex(x,y)];
        }
        set {
            m_values[GetFlatIndex(x,y)] = value;
        }
    }

    public bool InBounds(int2 coord){
        return InBounds(coord.x, coord.y);
    }

    public bool InBounds(int x, int y){
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    public T this[int2 coord]{
        get { return this[coord.x, coord.y]; }
        set { this[coord.x, coord.y] = value; }
    }

    public T this[Vector2Int coord]{
        get { return this[coord.x, coord.y]; }
        set { this[coord.x, coord.y] = value; }
    }

}

}