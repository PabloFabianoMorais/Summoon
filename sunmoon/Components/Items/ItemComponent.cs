using sunmoon.Core.ECS;

namespace sunmoon.Components.Items
{
    public enum ItemType
    {

        Weapon,
        Armor,
        Consumable,
        Backpack,
        Misc
    }
    public enum EquipmentSlot
    {
        None,
        Hand,
        Head,
        Chest,
        Legs,
        Backpack
    }
    public class ItemComponent : Component
    {
        public string Name { get; set; }
        public float Weight { get; set; }
        public ItemType ItemType { get; set; }
        public EquipmentSlot EquipmentSlot { get; set; }
    }
}