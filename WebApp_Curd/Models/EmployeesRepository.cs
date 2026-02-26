namespace WebApp_Curd.Models
{
    public class EmployeesRepository
    {
        private static List<Employee> _employeesList = new List<Employee>
        {
            new Employee(1,"John Doe","Engineer",60000),
            new Employee(2,"Jane Smith","Manger",75000),
            new Employee(3,"Sam Brown","Technicain",50000)
        };

        public static List<Employee> GetEmployees() { return _employeesList; }

        public static Employee? GetEmployeeById(int id)
        {
            return _employeesList.FirstOrDefault(x => x.Id == id);
        }



        public static void AddEmployee(Employee? emp)
        {
            if (emp is not null)
            {
                _employeesList.Add(emp);
            }
        }

        public static void DeleteEmployeeById(int id)
        {
            if (id <= 0) return;

            Employee emp = _employeesList.FirstOrDefault(e => e.Id == id);

            if (emp != null)
            {
                _employeesList.Remove(emp);
            }
        }

        public static bool UpdateEmployee(Employee employee)
        {
            Employee emp = _employeesList.FirstOrDefault(e => e.Id == employee.Id);

            if (emp == null)
                return false;

            emp.Name = employee.Name;
            emp.Position = employee.Position;
            emp.Salary = employee.Salary;

            return true;
        }
    }
}
