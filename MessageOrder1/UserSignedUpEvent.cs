namespace MessageOrder;

public record UserSignedUpEvent(Guid UserId, string Email, string Username);