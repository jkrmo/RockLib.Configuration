using System.Collections;
using System.Collections.Generic;

namespace System.Configuration
{
    internal interface IConfigurationElement
    {
        object this[string key] { get; set; }
    }

    internal abstract class ConfigurationElement : IConfigurationElement// : IDictionary<string, object>
    {
        private readonly Dictionary<string, object> _map = new Dictionary<string, object>();

        public virtual void SetElementKey(string key) { }

        /// <summary>
        /// Gets or sets a configuration value.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <value>The configuration value.</value>
        protected virtual object this[string key]
        {
            get => _map.ContainsKey(key) ? _map[key] : null;
            set
            {
                CheckValue(key, ref value);
                _map[key] = value;
            }
        }

        protected int Count => _map.Count;

        protected void Clear() => _map.Clear();

        object IConfigurationElement.this[string key]
        {
            get => this[key];
            set => this[key] = value;
        }

        protected virtual void CheckValue(string key, ref object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
        }

        #region IDictionary<string, object>

        //object IDictionary<string, object>.this[string key] { get => _map[key]; set => this[key] = value; }

        //ICollection<string> IDictionary<string, object>.Keys => _map.Keys;

        //ICollection<object> IDictionary<string, object>.Values => _map.Values;

        //int ICollection<KeyValuePair<string, object>>.Count => _map.Count;

        //bool ICollection<KeyValuePair<string, object>>.IsReadOnly => ((ICollection<KeyValuePair<string, object>>)_map).IsReadOnly;

        //void IDictionary<string, object>.Add(string key, object value)
        //{
        //    _map.Add(key, value);
        //}

        //void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        //{
        //    ((ICollection<KeyValuePair<string, object>>)_map).Add(item);
        //}

        //void ICollection<KeyValuePair<string, object>>.Clear()
        //{
        //    _map.Clear();
        //}

        //bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        //{
        //    return ((ICollection<KeyValuePair<string, object>>)_map).Contains(item);
        //}

        //bool IDictionary<string, object>.ContainsKey(string key)
        //{
        //    return _map.ContainsKey(key);
        //}

        //void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        //{
        //    ((ICollection<KeyValuePair<string, object>>)_map).CopyTo(array, arrayIndex);
        //}

        //IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        //{
        //    return _map.GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return ((IEnumerable<KeyValuePair<string, object>>)_map).GetEnumerator();
        //}

        //bool IDictionary<string, object>.Remove(string key)
        //{
        //    return _map.Remove(key);
        //}

        //bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        //{
        //    return ((ICollection<KeyValuePair<string, object>>)_map).Remove(item);
        //}

        //bool IDictionary<string, object>.TryGetValue(string key, out object value)
        //{
        //    return _map.TryGetValue(key, out value);
        //}

        #endregion
    }
}
