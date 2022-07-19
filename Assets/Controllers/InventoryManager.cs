using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ScriptableObjects;
using UnityEngine;

namespace Controllers
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager current;
        
        private readonly Dictionary<InventoryItemData, InventoryItem> _items = new();
        private readonly List<InventoryItem> _inventory = new();

        private void Awake()
        {
            current = this;
        }

        [CanBeNull]
        public InventoryItem Get(InventoryItemData itemData)
        {
            _items.TryGetValue(itemData, out var item);
            return item;
        }

        public void Add(InventoryItemData itemData)
        {
            if (_items.TryGetValue(itemData, out var item))
            {
                item.AddToStack();
            }
            else
            {
                var newItem = new InventoryItem(itemData);
                _items[itemData] = newItem;
                _inventory.Add(newItem);
            }
        }

        public void Remove(InventoryItemData itemData)
        {
            if (!_items.TryGetValue(itemData, out var item)) return;

            item.RemoveFromStack();
            if (!item.IsStackEmpty) return;

            _items.Remove(itemData);
            _inventory.Remove(item);
        }
    }
}