using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Core.Services;
namespace sunmoon.UI
{
    public class DebugPanel : UIPanel
    {
        public DebugPanel(SpriteFont font) : base(new Point(0, 0), Vector2.Zero, new Color(255, 255, 255, 0.3f))
        {
            var stackPanel = new UIStackPanel(new Vector2(10, 10), 5f);
            AddChild(stackPanel);

            var dayCountLabel = new UILabel(font, () => $"Dia: {DebugService.DayCount}", Vector2.Zero, Color.White);
            var currentTimeLabel = new UILabel(font, () => $"Horário: {DebugService.CurrentTime:F2}", Vector2.Zero, Color.White);
            var playerPosLabel = new UILabel(font, () => $"Position: X={DebugService.PlayerPosition.X:F0}, Y={DebugService.PlayerPosition.Y:F0}", Vector2.Zero, Color.White);
            var fpsLabel = new UILabel(font, () => $"FPS: {DebugService.Fps:F0}", Vector2.Zero, Color.White);
            var objectCountLabel = new UILabel(font, () => $"Objetos: {DebugService.ObjectsCount}", Vector2.Zero, Color.White);
            var renderedObjectsLabel = new UILabel(font, () => $"Objetos renderizados: {DebugService.RenderedObjects}", Vector2.Zero, Color.White);
            var renderedChunksLabel = new UILabel(font, () => $"Chunks renderizadas: {DebugService.RenderedChunks}", Vector2.Zero, Color.White);

            stackPanel.AddChild(dayCountLabel);
            stackPanel.AddChild(currentTimeLabel);
            stackPanel.AddChild(playerPosLabel);
            stackPanel.AddChild(fpsLabel);
            stackPanel.AddChild(objectCountLabel);
            stackPanel.AddChild(renderedObjectsLabel);
            stackPanel.AddChild(renderedChunksLabel);

            IsVisible = false;
        }
    }
}