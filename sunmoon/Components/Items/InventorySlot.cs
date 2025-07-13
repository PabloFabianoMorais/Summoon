using sunmoon.Core.ECS;

namespace sunmoon.Components.Items
{
    public class InventorySlot
    {
        public GameObject Item { get; private set; }
        public int Count { get; set; }

        public InventorySlot(GameObject item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}