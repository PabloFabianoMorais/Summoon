namespace sunmoon.Core.ECS
{
    public interface IComponent
    {
        public GameObject GameObject { get; set; }

        void Initialize() { }
    }
}