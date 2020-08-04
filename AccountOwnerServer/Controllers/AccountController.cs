using Contracts;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Threading.Tasks;

namespace AccountOwnerServer.Controllers
{
    [Route("api/acc")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public AccountController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            try
            {
                var accounts = await _repository.Account.GetAllAccountsAsync();

                _logger.LogInfo($"Returned all accounts from database.");

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllAccounts action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "AccountById")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            try
            {
                var account = await _repository.Account.GetAccountByIdAsync(id);

                if (account.IsEmptyObject())
                {
                    _logger.LogError($"Account with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned account with id: {id}");
                    return Ok(account);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAccountById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/account")]
        public async Task<IActionResult> GetAccountWithDetails(Guid id)
        {
            try
            {
                var account = await _repository.Account.GetAccountWithDetailsAsync(id);

                if (account.IsEmptyObject())
                {
                    _logger.LogError($"Account with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned account with details for id: {id}");
                    return Ok(account);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAccountWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] Account account)
        {
            try
            {
                if (account.IsObjectNull())
                {
                    _logger.LogError("Account object sent from client is null.");
                    return BadRequest("Account object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid account object sent from client");
                    return BadRequest("Invalid model object");
                }

                await _repository.Account.CreateAccountAsync(account);

                return CreatedAtRoute("AccountById", new { id = account.Id }, account);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] Account account)
        {
            try
            {
                if (account.IsObjectNull())
                {
                    _logger.LogError("Account object sent from client is null.");
                    return BadRequest("Account object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid account object sent from client");
                    return BadRequest("Invalid model object");
                }

                var dbAccount = await _repository.Account.GetAccountByIdAsync(id);
                if (dbAccount.IsEmptyObject())
                {
                    _logger.LogError($"Account with id {id} not found in db");
                    return NotFound();
                }

                await _repository.Account.UpdateAccountAsync(dbAccount, account);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try
            {
                var account = await _repository.Account.GetAccountByIdAsync(id);

                if (account.IsEmptyObject())
                {
                    _logger.LogError($"Account with id {id} not found in db");
                    return NotFound();
                }

                if (_repository.Account.AccountsByOwner(id).Any())
                {
                    _logger.LogError($"Cannot delete owner with id {id}. Account has existing accounts. Delete those accounts first.");
                    return BadRequest($"Cannot delete owner with id {id}. Account has existing accounts. Delete those accounts first.");
                }
                await _repository.Account.DeleteAccountAsync(account);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}