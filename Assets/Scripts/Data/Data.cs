using System;

public class Data : ICloneable
{
    public int Id { get; set; }
    public DataTable TableModel { get; set; }

    public Action<Data> OnDataRemove;
    public void RemoveData()
    {
        OnDataRemove?.Invoke(this);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
