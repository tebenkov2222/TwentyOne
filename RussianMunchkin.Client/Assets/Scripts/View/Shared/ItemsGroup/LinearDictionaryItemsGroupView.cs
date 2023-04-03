using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View.Shared.ItemsGroup
{
    public class LinearDictionaryItemsGroupView<T>: MonoBehaviour, IEnumerable<KeyValuePair<int, T>> where T: MonoBehaviour
    {
        [SerializeField] private Transform _contentRoot;
        
        private Dictionary<int, T> _items = new Dictionary<int, T>();

        public Dictionary<int, T> Items => _items;

        public T this[int key]
        {
            get => _items[key];
        }

        public int Count => _items.Count;

        public TPrefab Add<TPrefab>(int id, TPrefab prefab) where TPrefab : T
        {
            var item = Instantiate(prefab, _contentRoot);
            item.gameObject.SetActive(true);
            _items.Add(id, item);
            return item;
        }

        public void RemoveItem(int itemId)
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

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}