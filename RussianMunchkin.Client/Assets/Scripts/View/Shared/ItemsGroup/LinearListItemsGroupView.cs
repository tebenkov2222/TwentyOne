using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View.Shared.ItemsGroup
{
    public abstract class LinearListItemsGroupView<T>: MonoBehaviour, IEnumerable<T> where T: MonoBehaviour
    {
        [SerializeField] private Transform _contentRoot;
        
        private List<T> _items = new List<T>();
        
        public T this[int key]
        {
            get => _items[key];
        }
        public int Count => _items.Count;

        public List<T> Items => _items;
        public virtual TPrefab Add<TPrefab>(TPrefab prefab) where TPrefab : T
        {
            var item = Instantiate(prefab, _contentRoot);
            item.gameObject.SetActive(true);
            _items.Add(item);
            return item;
        }

        public void RemoveItem(int itemId)
        {
            RemoveItem(_items[itemId]);
        }
        public virtual void RemoveItem(T item)
        {
            Destroy(item.gameObject);
            _items.Remove(item);
        }

        public virtual void RemoveAll()
        {
            var removeItems = _items.ToList();
            foreach (var item in removeItems)
            {
                RemoveItem(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}