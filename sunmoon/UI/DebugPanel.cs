using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Components;
using sunmoon.Core.Management;
using sunmoon.Core.Services;

namespace sunmoon.UI
{
    public class DebugPanel : UIPanel
    {
        public DebugPanel(SpriteFont font, TransformComponent playerTransform, GameObjectManager gameObjectManager, TilemapManager tilemapManager) : base(new Point(300, 400), new Vector2(0, 0), new Color(0, 0, 0, 100))
        {
            var stackPanel = new UIStackPanel(new Vector2(10, 10), 5f);
            AddChild(stackPanel);

            var playerPosLabel = new UILabel(font, () => $"Position: X={playerTransform.Position.X:F0}, Y={playerTransform.Position.X:F0}", Vector2.Zero, Color.White);
            var fpsLabel = new UILabel(font, () => $"FPS: {DebugService.Fps:F0}", Vector2.Zero, Color.White);
            var objectCountLabel = new UILabel(font, () => $"Objetos: {gameObjectManager.GetObjectsCount()}", Vector2.Zero, Color.White);
            var renderedObjectsLabel = new UILabel(font, () => $"Objetos renderizados: {gameObjectManager.GetRenderedObjectsCount()}", Vector2.Zero, Color.White);
            var renderedChunksLabel = new UILabel(font, () => $"Chunks renderizadas: {tilemapManager.GetRenderedChunksCount()}", Vector2.Zero, Color.White);



            stackPanel.AddChild(playerPosLabel);
            stackPanel.AddChild(fpsLabel);
            stackPanel.AddChild(objectCountLabel);
            stackPanel.AddChild(renderedObjectsLabel);
            stackPanel.AddChild(renderedChunksLabel);

            IsVisible = false;
        }
    }
}