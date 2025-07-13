using sunmoon.Core.ECS;

namespace sunmoon.Components.Items
{
    public class EquipmentComponent : Component
    {
        public GameObject RightHand { get; private set; }
        public GameObject LeftHand { get; private set; }
        public GameObject Backpack { get; private set; }
        public GameObject Head { get; private set; }
        public GameObject Chest { get; private set; }
        public GameObject Legs { get; private set; }

        public bool EquipItem(GameObject itemToEquip)
        {
            var itemComponent = itemToEquip.GetComponent<ItemComponent>();
            if (itemComponent == null) return false;

            switch (itemComponent.EquipmentSlot)
            {
                case EquipmentSlot.Hand:
                    if (RightHand == null) {RightHand = itemToEquip; return true; }
                    else if (LeftHand == null) {LeftHand = itemToEquip; return true; }
                    break;
                case EquipmentSlot.Backpack:
                    if (Backpack == null) {Backpack = itemToEquip; return true;}
                    break;
                case EquipmentSlot.Head:
                    if (Head == null) {Head = itemToEquip; return true;}
                    break;
                case EquipmentSlot.Chest:
                    if (Chest == null) {Chest = itemToEquip; return true;}
                    break;
                case EquipmentSlot.Legs:
                    if (Legs == null) {Legs = itemToEquip; return true;}
                    break;
            }

            return false;
        }
    }
}