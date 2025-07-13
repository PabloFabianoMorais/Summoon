
using System.Linq.Expressions;
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

            string rightHandText;
            if (rightHand != null) rightHandText = $"Mão direita: {rightHand.GetComponent<ItemComponent>().Name}";
            else rightHandText = $"Mão direita: Nada";
            string leftHandText;
            if (leftHand != null) leftHandText = $"Mão esquerda: {leftHand.GetComponent<ItemComponent>().Name}";
            else leftHandText = $"Mão esquerda: Nada";

            var rightHandLabel = new UILabel(font, () => rightHandText, Vector2.Zero, Color.White);
            var leftHandLabel = new UILabel(font, () => leftHandText, Vector2.Zero, Color.White);


            stackPanel.AddChild(rightHandLabel);
            stackPanel.AddChild(leftHandLabel);

            AddChild(stackPanel);

            IsVisible = false;
        }
    }
}