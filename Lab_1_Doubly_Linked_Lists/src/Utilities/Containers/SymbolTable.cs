//make this class read only
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class SymbolTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    private DLL<TKey> _keys;
    private DLL<TValue> _values;
    private int _size;
    public SymbolTable()
    {
        this._keys = new DLL<TKey>();
        this._values = new DLL<TValue>();
        this._size = 0;
    }
    bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
    {
        throw new NotImplementedException();
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        this._keys.Add(item.Key);
        this._values.Add(item.Value);
        this._size++;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        int index = this._keys.IndexOf(item.Key);
        this._keys.RemoveAt(index);
        this._values.RemoveAt(index);
        this._size--;
        return true;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(TKey key, TValue value)
    {
        throw new NotImplementedException();
    }

    public bool ContainsKey(TKey key)
    {
        throw new NotImplementedException();
    }

    public bool Remove(TKey key)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public ICollection<TKey> Keys => throw new NotImplementedException();

    public ICollection<TValue> Values => throw new NotImplementedException();

    public int Count => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public TValue this[TKey key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

//Visual Studio quickfix for Type T
public class T
{
}