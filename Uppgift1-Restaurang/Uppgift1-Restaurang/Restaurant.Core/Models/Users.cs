namespace Restaurant.Core.Models
{
    class Users
    {
        private List<User> _users = new List<User>();

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public List<User> LazyInitializer()
        {
            _users = new List<User>();
            return _users;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

    }
}
