
namespace sunmoon.Core.ECS
{
    /// <summary>
    /// A classe base para todos os componentes no sistema ECS.
    /// Define um 'pedaço' de dados ou lógica que pode ser anexado a um GameObject.
    /// Esta classe deve ser herdada, não instanciada diretamente.
    /// </summary>
    public class Component : IComponent
    {

        public GameObject GameObject { get; set; }

        public virtual void Initialize() { }
    }
}