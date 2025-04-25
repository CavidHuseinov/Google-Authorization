
namespace Auth.Core.Entities.Common
{
    public class BaseEntity
    {
        public Guid Id {  get; set; } = Guid.NewGuid();
    }
}
