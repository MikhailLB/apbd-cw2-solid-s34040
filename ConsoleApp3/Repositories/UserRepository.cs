namespace ConsoleApp3.Repositories;

using Models;

public class UserRepository
{
    private readonly List<User> _users = new();

    public void Add(User user) => _users.Add(user);

    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

    public List<User> GetAll() => _users.ToList();
}
