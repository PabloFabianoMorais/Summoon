using Microsoft.Xna.Framework.Content;

namespace sunmoon.Core.ECS
{
    public interface IContentLoadable
    {
        void LoadContent(ContentManager contentManager);
    }
}