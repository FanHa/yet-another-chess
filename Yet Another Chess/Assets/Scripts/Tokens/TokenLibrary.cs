using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "TokenLibrary.asset", menuName = "YetAnotherChess/Token Library", order = 1)]
public class TokenLibrary : ScriptableObject, IList<Token>, IDictionary<string, Token>
{
    public List<Token> tokens;
    Dictionary<string, Token> m_TokenDictionary;
 

    public void AfterDeserialize()
    {
        if (tokens == null)
        {
            return;
        }
        m_TokenDictionary = tokens.ToDictionary(t => t.Name);
    }
    public bool ContainsKey(string key)
    {
        return m_TokenDictionary.ContainsKey(key);
    }

    public void Add(string key, Token value)
    {
        m_TokenDictionary.Add(key, value);
    }

    public bool TryGetValue(string key, out Token value)
    {
        return m_TokenDictionary.TryGetValue(key, out value);
    }

    Token IDictionary<string, Token>.this[string key]
    {
        get { return m_TokenDictionary[key]; }
        set { m_TokenDictionary[key] = value; }
    }

    public ICollection<string> Keys
    {
        get { return ((IDictionary<string, Token>)m_TokenDictionary).Keys; }
    }

    public bool Remove(string key)
    {
        return m_TokenDictionary.Remove(key);
    }


    ICollection<Token> IDictionary<string, Token>.Values
    {
        get { return m_TokenDictionary.Values; }
    }

    IEnumerator<KeyValuePair<string, Token>> IEnumerable<KeyValuePair<string, Token>>.GetEnumerator()
    {
        return m_TokenDictionary.GetEnumerator();
    }

    public void Add(KeyValuePair<string, Token> item)
    {
        m_TokenDictionary.Add(item.Key, item.Value);
    }

    public bool Remove(KeyValuePair<string, Token> item)
    {
        return m_TokenDictionary.Remove(item.Key);
    }
    public bool Contains(KeyValuePair<string, Token> item)
    {
        return m_TokenDictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, Token>[] array, int arrayIndex)
    {
        int count = array.Length;
        for (int i = arrayIndex; i < count; i++)
        {
            Token config = tokens[i - arrayIndex];
            KeyValuePair<string, Token> current = new KeyValuePair<string, Token>(config.Name, config);
            array[i] = current;
        }
    }

    public int IndexOf(Token item)
    {
        return tokens.IndexOf(item);
    }

    public void Insert(int index, Token item)
    {
        tokens.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        tokens.RemoveAt(index);
    }

    Token IList<Token>.this[int index]
    {
        get { return tokens[index]; }
        set { tokens[index] = value; }
    }

    public void Add(Token item)
    {
        tokens.Add(item);
    }

    public void Clear()
    {
        tokens.Clear();
    }

    public bool Contains(Token item)
    {
        return tokens.Contains(item);
    }

    public void CopyTo(Token[] array, int arrayIndex)
    {
        tokens.CopyTo(array, arrayIndex);
    }

    public bool Remove(Token item)
    {
        return tokens.Remove(item);
    }

    public int Count
    {
        get { return tokens.Count; }
    }

    public bool IsReadOnly
    {
        get { return ((ICollection<Token>)tokens).IsReadOnly; }
    }

    public IEnumerator<Token> GetEnumerator()
    {
        return tokens.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)tokens).GetEnumerator();
    }

}
