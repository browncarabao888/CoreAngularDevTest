using CoreAngularDevTest.Server.Interfaces;
using CoreAngularDevTest.Server.Models;
using CoreAngularDevTest.Server.Models.DTO.Accounts;
using Microsoft.EntityFrameworkCore;

namespace CoreAngularDevTest.Server.Services
{
    public class UserService : IUser
    {
        #region Variables
        private readonly ApplicationDbContext _context;
        private readonly IServiceScopeFactory _scopeFactory;
        #endregion

        #region Constructor
        public UserService(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _scopeFactory = scopeFactory;
        }
        #endregion

        

        public async Task<int?> AddAccountsAsync(AccountDTO newAccount, CancellationToken token)
        {

            var result = 0;
            try
            {
                token.ThrowIfCancellationRequested();

                if (newAccount == null)
                    throw new ArgumentNullException(nameof(newAccount));

                var account = new Account
                {
                    FirstName = newAccount.FirstName,
                    LastName = newAccount.LastName,
                    Emailaddress = newAccount.Emailaddress,
                    Passkey = newAccount.Passkey,
                    UserName = newAccount.UserName,
                    Datecreated = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Status = 1
                };
             
               await _context.Accounts.AddAsync(account);
                result = await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public async Task<bool> IsEmailConsumed(string? emailaddress, CancellationToken token)
        {
            var result = true;

            try
            {
                token.ThrowIfCancellationRequested();
                var value =  await _context.Accounts.Where(e => e.Emailaddress == emailaddress).FirstOrDefaultAsync();
                result = value != null;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public async Task<int> ResetKeyAsync(string emailaddress, CancellationToken token)
        {
            var result = 0;

            try
            {
                var accountObj = await _context.Accounts.Where(e => e.Emailaddress == emailaddress).FirstOrDefaultAsync();

                if (accountObj == null)
                    return result;

                if (accountObj != null)
                {
                    accountObj.Passkey = "";

                   _context.Accounts.Update(accountObj);
                    result = await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public Task<int> ValidateAsync(LoginDTO loginDTO, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
