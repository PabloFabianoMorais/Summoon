
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Components.Items;

namespace sunmoon.UI
{
    public class PlayerEquipmentPanel : UIPanel
    {
        public PlayerEquipmentPanel(SpriteFont font, EquipmentComponent equipmentComponent) : base(new Point(300, 300), new Vector2(30, 30), new Color(0, 0, 0, 100))
        {
            var stackPanel = new UIStackPanel(new Vector2(10, 10), 10f);

            var rightHand = equipmentComponent.RightHand;
            var leftHand = equipmentComponent.LeftHand;

            var rightHandLabel = new UILabel(font, () => $"Mão direita: {equipmentComponent.RightHand?.GetComponent<ItemComponent>().Name}", Vector2.Zero, Color.White);
            var leftHandLabel = new UILabel(font, () => $"Mão esquerda: {equipmentComponent.LeftHand?.GetComponent<ItemComponent>().Name}", Vector2.Zero, Color.White);


            stackPanel.AddChild(rightHandLabel);
            stackPanel.AddChild(leftHandLabel);

            AddChild(stackPanel);

            IsVisible = false;
        }
    }
}