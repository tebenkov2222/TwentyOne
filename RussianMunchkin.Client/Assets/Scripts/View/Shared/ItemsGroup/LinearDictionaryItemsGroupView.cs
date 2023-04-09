using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View.Shared.ItemsGroup
{
    public class LinearDictionaryItemsGroupView<T>: MonoBehaviour, IEnumerable<KeyValuePair<string, T>> where T: MonoBehaviour
    {
        [SerializeField] private Transform _contentRoot;
        
        private Dictionary<string, T> _items = new Dictionary<string, T>();

        public Dictionary<string, T> Items => _items;

        public T this[string key]
        {
            get => _items[key];
        }

        public int Count => _items.Count;

        public TPrefab Add<TPrefab>(string id, TPrefab prefab) where TPrefab : T
        {
            var item = Instantiate(prefab, _contentRoot);
            item.gameObject.SetActive(true);
            Debug.Log($"Add to dictionary. id = |{id}|, value = |{item}|");
            _items.Add(id, item);
            return item;
        }

        public void RemoveItem(string itemId)
        {
            _items.Remove(itemId, out var item);
            Destroy(item.gameObject);
        }

        public void RemoveAll()
        {
            var removeItems = _items.Keys.ToList();
            foreach (var item in removeItems)
            {
                RemoveItem(item);
            }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}