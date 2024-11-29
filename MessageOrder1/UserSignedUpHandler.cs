namespace MessageOrder;

public class UserSignedUpHandler(IUserRepository repository)
{
    public void Handle(UserSignedUpEvent @event)
    {
        var newUser = new User()
        {
            Id = @event.UserId,
            Email = @event.Email,
            Username = @event.Username
        };
        
        repository.Save(newUser);
    }
}