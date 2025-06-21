using CoreAngularDevTest.Server.Models.DTO.Accounts;

namespace CoreAngularDevTest.Server.Interfaces
{
    public interface IUser
    {
       
        Task<int?> AddAccountsAsync(AccountDTO? newAccount, CancellationToken token);
        Task<bool> IsEmailConsumed(string? emailaddress, CancellationToken token);
        Task<int> ResetKeyAsync(string passkey, CancellationToken token);
        Task<int> ValidateAsync(LoginDTO? loginDTO, CancellationToken token);
    }
}
