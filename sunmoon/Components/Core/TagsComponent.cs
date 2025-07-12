
using System.Collections.Generic;
using sunmoon.Core.ECS;

namespace sunmoon.Components.Core
{
    public class TagsComponent : Component
    {
        public HashSet<string> Tags { get; set; } = new HashSet<string>();
    }
}