
namespace sunmoon.Core.ECS
{
    public class Component : IComponent
    {

        public GameObject GameObject { get; set; }

        public virtual void Initialize() {}
    }
}