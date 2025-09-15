//make this class read only
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class SymbolTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    private DLL<TKey> _keys;
    private DLL<TValue> _values;
    private int _size;
    public void SymbolTableConstruct()
    {
        this._keys = new DLL<TKey>();
        this._values = new DLL<TValue>();
        this._size = 0;
    }

    public void Add(TKey key, TValue value)
    {
        this._keys.Add(key);
        this._values.Add(value);
        this._size++;
    }

    public bool Remove(TKey key) //impliment item not found for a false return
    {
        int index = this._keys.IndexOf(key);
        this._keys.RemoveAt(index);
        this._values.RemoveAt(index);
        this._size--;
        return true;

    }

    public void Clear()
    {
        this._keys.Clear();
        this._values.Clear();
        this._size = 0;
    }

    public bool Contains(TValue value) { return this._values.Contains(value); }
    public bool ContainsKey(TKey key) { return this._keys.Contains(key); }

    public void CopyTo(T[] array, int index)
    {
        //skipped for now
    }

    bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
    {
        throw new NotImplementedException();
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool TryGetValue
    {

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