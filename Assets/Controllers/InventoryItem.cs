using ScriptableObjects;

namespace Controllers
{
    public class InventoryItem
    {
        public readonly InventoryItemData Data;
        public int StackSize { get; private set; }
        public bool IsStackEmpty => StackSize == 0;

        public InventoryItem(InventoryItemData data)
        {
            Data = data;
            StackSize = StackSize;
        }

        public void AddToStack()
        {
            StackSize++;
        }

        public void RemoveFromStack()
        {
            StackSize--;
        }
    }
}