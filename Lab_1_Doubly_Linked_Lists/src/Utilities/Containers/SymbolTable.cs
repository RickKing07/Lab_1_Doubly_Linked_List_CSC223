//make this class read only
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

public class SymbolTable<TKey, TValue> : IDictionary<TKey, TValue> //make parent proerty
{

    private DLL<TKey> _keys;
    private DLL<TValue> _values;
    private int _size;
    public SymbolTable<TKey, TValue> _parent;
    public SymbolTable()
    {
        this._keys = new DLL<TKey>();
        this._values = new DLL<TValue>();
        this._size = 0;
        this._parent = null;

    }

    public SymbolTable(SymbolTable<TKey, TValue> parent)
    {
        this._parent = parent;
        this._keys = new DLL<TKey>();
        this._values = new DLL<TValue>();
        this._size = 0;
    }
    bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
    {
        TValue LocalValue = default(TValue);
        if (this.TryGetValueLocal(key, out LocalValue))
        {
            value = LocalValue;
            return true;
        }
        if (this._parent != null)
        {
            return ((IDictionary<TKey, TValue>)this._parent).TryGetValue(key, out value);
        }
        value = default(TValue);
        return false;
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        this._keys.Add(item.Key);
        this._values.Add(item.Value);
        this._size++;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        int index = this._keys.IndexOf(item.Key);
        TValue value = this._values[index];
        return index != -1 && value.Equals(item.Value);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        int Index = 0;
        if (array == null) throw new ArgumentNullException();
        if (arrayIndex < 0) throw new ArgumentOutOfRangeException();
        if (array.Length < this._size - arrayIndex) throw new ArgumentException();
        while (Index < this._size)
        {
            if (Index >= arrayIndex)
            {
                array[Index] = new KeyValuePair<TKey, TValue>(this._keys[Index], this._values[Index]);//TKey TValue pair; 
                Index++;
            }
            Index++;
        }
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
        int index = 0;
        while (index < this._size)
        {
            yield return new KeyValuePair<TKey, TValue>(this._keys[index], this._values[index]);
            index++;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(TKey key, TValue value)
    {
        this._keys.Add(key);
        this._values.Add(value);
        this._size++;
    }

    public bool ContainsKey(TKey key)
    {
        if (this._parent == null)
        {
            return ContainsKeyLocal(key);
        }
        else if (ContainsKeyLocal(key))
        {
            return true;
        }
        return this._parent.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        int index = this._keys.IndexOf(key);
        if (index != -1)
        {
            this._keys.RemoveAt(index);
            this._values.RemoveAt(index);
            this._size--;
            return true;
        }
        else return false;
    }

    public void Clear()
    {
        this._keys.Clear();
        this._values.Clear();
        this._size = 0;
    }

    public ICollection<TKey> Keys => this._keys;


    public ICollection<TValue> Values => this._values;

    public int Count => this._size;
    public bool IsReadOnly => false;

    public TValue this[TKey key]
    {
        get
        {

            int index = this._keys.IndexOf(key);
            if (index == -1) throw new KeyNotFoundException($"Key '{key}' not found");

            return this._values[index];
        }

        set
        {
            int index = this._keys.IndexOf(key);
            if (index == -1) throw new KeyNotFoundException($"Key '{key}' not found");
            this._values[index] = value;
        }

    }

    public bool ContainsKeyLocal(TKey key)
    {
        if (key == null) throw new ArgumentNullException("Null key");
        return this._keys.IndexOf(key) != -1;
    }

    public bool TryGetValueLocal(TKey key, out TValue value)
    {

        int index = this._keys.IndexOf(key);
        value = this._values[index];

        return index != -1;
    }
}


//Visual Studio quickfix for Type T
public class T
{ }
