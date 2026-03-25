namespace ConsoleApp3.Services;

using Models;
using Repositories;

public class UserService
{
    private readonly UserRepository _repository;

    public UserService(UserRepository repository)
    {
        _repository = repository;
    }

    public void Add(User user) => _repository.Add(user);

    public User? GetById(int id) => _repository.GetById(id);

    public List<User> GetAll() => _repository.GetAll();
}
