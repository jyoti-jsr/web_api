namespace WebApp_Curd.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
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
