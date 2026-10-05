namespace Restaurant.Core.Services
{
    using Restaurant.Core.Models;

    public interface IUserRegistry
    {
        void RegisterUser(User user);
        User GetUserByPersonalNumber(string personalNumber);
    }

    public class UserRegistry : IUserRegistry
    {
        private readonly Users users;

        public UserRegistry()
        {
            this.users = new Users();
            this.users.LazyInitializer();
        }

       UserRegistry(Users users)
        {
            this.users = users;
        }



        public void RegisterUser(User user)
        {
            users.AddUser(user);
        }
        public User GetUserByPersonalNumber(string personalNumber)
        {
            return users.GetAllUsers().FirstOrDefault(u => u.PersonalNumber == personalNumber);
        }
    }


}