namespace acid;

public class Account(AccountId id)
{
    public AccountId Id { get; set; } = id;
    public int Balance { get; set; }
}