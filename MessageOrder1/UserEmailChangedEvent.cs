namespace MessageOrder;

public record UserEmailChangedEvent(Guid UserId, string NewEmail);