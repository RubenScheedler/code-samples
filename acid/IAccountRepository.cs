namespace acid;

public interface IAccountRepository
{
    Account GetAccount(AccountId fromId);
    void Save(Account account);
}