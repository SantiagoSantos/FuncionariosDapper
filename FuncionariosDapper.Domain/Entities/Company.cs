
namespace FuncionariosDapper.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string? Address { get; set; }
        public string? Country { get; set; }
        public IEnumerable<Employee>? Employees { get; set; } = new List<Employee>();
    }
}
