using System.ComponentModel.DataAnnotations;

namespace WebApp_Curd.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Position { get; set; }
        [Required]
        [Range(50000, 200000)]
        [Employee_EnsureSalary]
        public double Salary { get; set; }


        public Employee(int id, string name, string position, double salary)
        {
            this.Id = id;
            this.Name = name;
            this.Position = position;
            this.Salary = salary;
        }
    }
}
