using System;

public interface IPoolable<T>
{
    public Action<T> Release { get; set; }
}