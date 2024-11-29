namespace MessageOrder;

public class UserEmailChangedHandler(IUserRepository repository)
{
    public void Handle(UserEmailChangedEvent @event)
    {
        var user = repository.Get(@event.UserId);
        
        user.Email = @event.NewEmail;
        
        repository.Save(user);
    }
}