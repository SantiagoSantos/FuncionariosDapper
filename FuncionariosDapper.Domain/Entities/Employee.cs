
namespace FuncionariosDapper.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string? Age { get; set; }
        public string? Position { get; set; }
        public int CompanyId { get; set; }
    }
}
