using System.Collections;
using System.Collections.Generic;

namespace System.Configuration
{
    internal abstract class ConfigurationElementCollection<TConfigurationElement> : ConfigurationElement, IList<TConfigurationElement>, ICollection
        where TConfigurationElement : ConfigurationElement
    {
        private readonly List<TConfigurationElement> _elements = new List<TConfigurationElement>();

        public TConfigurationElement this[int index] { get => _elements[index]; set => _elements[index] = value; }

        public int Count => _elements.Count;

        public bool IsReadOnly => ((IList<TConfigurationElement>)_elements).IsReadOnly;

        public bool IsSynchronized => ((ICollection)_elements).IsSynchronized;

        public object SyncRoot => ((ICollection)_elements).SyncRoot;

        public void Add(TConfigurationElement item) => _elements.Add(item);

        public void Clear() => _elements.Clear();

        public bool Contains(TConfigurationElement item) => _elements.Contains(item);

        public void CopyTo(TConfigurationElement[] array, int arrayIndex) => _elements.CopyTo(array, arrayIndex);

        public IEnumerator GetEnumerator() => ((IEnumerable)_elements).GetEnumerator();

        public int IndexOf(TConfigurationElement item) => _elements.IndexOf(item);

        public void Insert(int index, TConfigurationElement item) => _elements.Insert(index, item);

        public bool Remove(TConfigurationElement item) => _elements.Remove(item);

        public void RemoveAt(int index) => _elements.RemoveAt(index);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_elements).CopyTo(array, index);

        IEnumerator<TConfigurationElement> IEnumerable<TConfigurationElement>.GetEnumerator() => ((IList<TConfigurationElement>)_elements).GetEnumerator();
    }
}
