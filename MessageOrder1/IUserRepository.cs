namespace MessageOrder;

public interface IUserRepository
{
    User Get(Guid id);
    void Save(User user);
}