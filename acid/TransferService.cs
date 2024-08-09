namespace acid;

public class TransferService(IAccountRepository repository)
{
    public void TransferMoney(AccountId fromId, AccountId toId, int amount)
    {
        var fromAccount = repository.GetAccount(fromId);
        fromAccount.Balance -= amount;
        repository.Save(fromAccount);
        
        var toAccount = repository.GetAccount(toId);
        toAccount.Balance += amount;
        repository.Save(toAccount);
    }
}