using Armpit.Library.Utils;
using PassEncrypt = BCrypt.Net.BCrypt;
using WebServer.DatabaseManagement.Auth;
using WebServer.DataModels.Account;
using WebServer.Logging;
using Microsoft.EntityFrameworkCore;

namespace WebServer.DatabaseManagement.Repositories;

public class AccountRepository
{

    #region Private vars

    private ArmpitAuthDbContext _authDbContext;

    private ILogger _logger;

    #endregion

    public AccountRepository(ArmpitAuthDbContext authDbContext, LoggersContainer loggersContainer)
    {
        _authDbContext = authDbContext;

        _logger = loggersContainer.ApplicationLogger;
    }

    public List<AccountInformation>? GetAllAccounts()
    {
        try
        {
            var accounts = _authDbContext.Accounts.ToList();
            accounts.ForEach(account => account.Password = string.Empty);
            return accounts;
        }
        catch (Exception ex)
        {
            _logger.LogError_ExceptionWithMethodName(ex);
            throw ArmpitException.GetExceptionWithMethodName(ex);
        }
    }

    public async Task<AccountInformation?> CreateAccount(AccountInformation account)
    {
        try
        {
            account.Password = PassEncrypt.HashPassword(account.Password);
            var res = await _authDbContext.Accounts.AddAsync(account);
            _authDbContext.SaveChanges();
            return res.Entity;
        }
        catch (Exception ex)
        {
            _logger.LogError_ExceptionWithMethodName(ex);
            throw ArmpitException.GetExceptionWithMethodName(ex);
        }
    }

    public AccountInformation? UpdateAccount(AccountInformation account)
    {
        try
        {
            if (account.Password != string.Empty)
                account.Password = PassEncrypt.HashPassword(account.Password);

            var res = _authDbContext.Accounts.Update(account);
            _authDbContext.SaveChanges();
            return res.Entity;
        }
        catch (Exception ex)
        {
            _logger.LogError_ExceptionWithMethodName(ex);
            throw ArmpitException.GetExceptionWithMethodName(ex);
        }
    }

    public void DeleteAccount(int id)
    {
        try
        {
            var account = _authDbContext.Accounts.FirstOrDefault(x => x.Id == id);
            if (account == null)
                throw new ArmpitException("Account to be deleted doesn't exist.");
            _authDbContext.Accounts.Remove(account);
            _authDbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            if (ex is ArmpitException)
                throw;
            _logger.LogError_ExceptionWithMethodName(ex);
            throw ArmpitException.GetExceptionWithMethodName(ex);
        }
    }

    /// <summary>
    /// Authentication method which throws <see cref="ArmpitException"/> if incorrect credentials were supplied.
    /// </summary>
    /// <param name="credentials"></param>
    /// <exception cref="ArmpitException"></exception>
    public void ThrowOnIncorrectCreds(AuthCredentials credentials)
    {
        try
        {
            var user = _authDbContext.Accounts.FirstOrDefault(a => a.Username == credentials.Username);
            if (user == null)
                throw new ArmpitException("Bad Credentials");

            if (!PassEncrypt.Verify(credentials.Password, user.Password))
                throw new ArmpitException("Bad Credentials");
        }
        catch (Exception ex)
        {
            _logger.LogError_ExceptionWithMethodName(ex);
            if (ex is ArmpitException)
                throw;

            throw new ArmpitException("Exception caught while trying to authenticate user.");
        }
    }
}
