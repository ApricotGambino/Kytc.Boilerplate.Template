namespace Domain.Interfaces.Features.Logging;

public interface ILoggerService
{
    public Task SendEmailAsync();
}



internal interface IBankAccount
{
    public void AddMoney(int money);
}

internal class BankAccount : IBankAccount
{
    public void AddMoney(int amount) // Noncompliant: parameter's name differs from base
    {
        // ...
    }
}
