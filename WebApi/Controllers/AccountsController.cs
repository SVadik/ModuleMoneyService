using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Account> GetUserAccounts()
        {
            var accounts = _service.GetUserAccounts(Convert.ToInt32(User.Identity.Name));

            return accounts;
        }

        [HttpPost]
        public IActionResult Register([FromBody]AccountModel model)
        {
            var account = _service.Create(new Account(Convert.ToInt32(User.Identity.Name)));

            return Ok();
        }

        [HttpPost("fill")]
        public IActionResult Fill([FromBody]TransactionModel model)
        {
            var account = _service.GetUserAccount(Convert.ToInt32(User.Identity.Name), model.FromNumber);
            if (account == null)
            {
                return BadRequest(new { message = "This account does not belong to user" });
            }
            account.Balance += model.Value;
            _service.Update(account);

            return Ok();
        }

        [HttpPost("send")]
        public IActionResult Send([FromBody]TransactionModel model)
        {
            //Переписать в метод сервиса
            var dbAccount = _service.GetUserAccount(Convert.ToInt32(User.Identity.Name), model.FromNumber);
            if (dbAccount == null)
            {
                return BadRequest(new { message = "This account does not exist" });
            }

            if(dbAccount.Balance < model.Value)
            {
                return BadRequest(new { message = "Not enough money" });
            }
            var receivingAccount = _service.Get(model.ToNumber);
            if (receivingAccount == null)
            {
                return BadRequest(new { message = "Receiving account does not exist" });
            }

            dbAccount.Balance -= model.Value;
            receivingAccount.Balance += model.Value;
            _service.Update(dbAccount);
            _service.Update(receivingAccount);

            return Ok();
        }
    }
}