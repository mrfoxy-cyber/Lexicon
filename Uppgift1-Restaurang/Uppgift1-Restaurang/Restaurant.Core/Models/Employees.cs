namespace Restaurant.Core.Models
{
    public class Employees
    {
        private List<Employee> _users = new List<Employee>();

        public List<Employee> GetAllUsers()
        {
            return _users;
        }

        public List<Employee> LazyInitializer()
        {
            _users = new List<Employee>();
            return _users;
        }

        public void AddUser(Employee user)
        {
            _users.Add(user);
        }

    }
}
