using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SOLA.Utilities
{
    [Serializable]
    public class ListDictionary<TKey, TValue> 
    {
        #region Fields

        [Serializable]
        public struct KeyValuePair
        {
            [HorizontalGroup(Width = 0.15f), LabelWidth(35)]
            public TKey Key;

            [HorizontalGroup(Width = 0.85f), LabelWidth(45)]
            public TValue Value;
        }

        [SerializeField]
        List<KeyValuePair> keyValuePairs = new List<KeyValuePair>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of key-value pairs contained in the dictionary.
        /// </summary>
        public int Count => keyValuePairs.Count;

        /// <summary>
        /// Gets the values contained in the dictionary.
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var pair in keyValuePairs)
                    yield return pair.Value;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Adds a key and value to the dictionary or updates the value if the key already exists.
        /// </summary>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value associated with the key.</param>
        public void Add(TKey key, TValue value)
        {
            var index = FindIndex(key);
            if (index != -1)
                keyValuePairs[index] = new KeyValuePair { Key = key, Value = value };
            else
                keyValuePairs.Add(new KeyValuePair { Key = key, Value = value });
        }

        /// <summary>
        /// Retrieves the value associated with the given key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <returns>The value associated with the key, or default if the key does not exist.</returns>
        public TValue Get(TKey key)
        {
            var index = FindIndex(key);
            if (index != -1)
                return keyValuePairs[index].Value;
            else
                return default(TValue);
        }

        /// <summary>
        /// Retrieves the key associated with a given value.
        /// </summary>
        /// <param name="value">The value to find the corresponding key for.</param>
        /// <returns>The key associated with the given value, or default if the value does not exist.</returns>
        public TKey Get(TValue value)
        {
            foreach (var pair in keyValuePairs)
            {
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
                    return pair.Key;
            }
            return default(TKey);
        }

        /// <summary>
        /// Tries to get the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGet(TKey key, out TValue value)
        {
            var index = FindIndex(key);
            if (index != -1)
            {
                value = keyValuePairs[index].Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        /// <summary>
        /// Tries to get the key associated with the specified value.
        /// </summary>
        /// <param name="value">The value whose key to find.</param>
        /// <param name="key">When this method returns, contains the key associated with the specified value, if the value is found; otherwise, the default value for the type of the key parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the dictionary contains an element with the specified value; otherwise, false.</returns>
        public bool TryGet(TValue value, out TKey key)
        {
            foreach (var pair in keyValuePairs)
            {
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
                {
                    key = pair.Key;
                    return true;
                }
            }
            key = default(TKey);
            return false;
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool Contains(TKey key)
        {
            return FindIndex(key) != -1;
        }


        /// <summary>
        /// Removes the key and its associated value from the dictionary, if it exists.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public void Remove(TKey key)
        {
            var index = FindIndex(key);
            if (index != -1)
                keyValuePairs.RemoveAt(index);
        }

        /// <summary>
        /// Clears all keys and values from the dictionary.
        /// </summary>
        public void Clear()
        {
            keyValuePairs.Clear();
        }

        /// <summary>
        /// Retrieves the key-value pair at the specified index.
        /// </summary>
        /// <param name="index">The index of the key-value pair to retrieve.</param>
        /// <returns>The key-value pair at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is out of range.</exception>
        public KeyValuePair GetByIndex(int index)
        {
            if (index < 0 || index >= keyValuePairs.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            return keyValuePairs[index];
        }

        /// <summary>
        /// Returns the index of the specified key in the dictionary, or -1 if not found.
        /// </summary>
        public int IndexOf(TKey key)
        {
            return FindIndex(key);
        }

        /// <summary>
        /// Returns the index of the specified value in the dictionary, or -1 if not found.
        /// </summary>
        public int IndexOf(TValue value)
        {
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                if (EqualityComparer<TValue>.Default.Equals(keyValuePairs[i].Value, value))
                    return i;
            }
            return -1;
        }

        #endregion

        #region Private 

        /// <summary>
        /// Finds the index of the key in the list of key-value pairs.
        /// </summary>
        /// <param name="key">The key to find.</param>
        /// <returns>The index of the key, or -1 if the key does not exist.</returns>
        int FindIndex(TKey key)
        {
            for (int i = 0; i < keyValuePairs.Count; i++)
                if (EqualityComparer<TKey>.Default.Equals(keyValuePairs[i].Key, key))
                    return i;
            return -1;
        }

        #endregion
    }
}
