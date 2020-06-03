namespace WebApi.UseCases.V1.Deposit
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.Boundaries.Deposit;
    using Domain.Accounts.ValueObjects;
    using FluentMediator;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Accounts
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Design-Patterns#controller">
    ///         Controller Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public sealed class AccountsController : ControllerBase
    {
        /// <summary>
        ///     Deposit on an account.
        /// </summary>
        /// <response code="200">The updated balance.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <param name="mediator"></param>
        /// <param name="getAccountsPresenter"></param>
        /// <param name="request">The request to deposit.</param>
        /// <returns>The updated balance.</returns>
        [Authorize]
        [HttpPatch("Deposit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DepositResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Deposit(
            [FromServices] IMediator mediator,
            [FromServices] DepositGetAccountsPresenter getAccountsPresenter,
            [FromForm] [Required] DepositRequest request)
        {
            var input = new DepositInput(
                new AccountId(request.AccountId),
                new PositiveMoney(request.Amount));

            await mediator.PublishAsync(input)
                .ConfigureAwait(false);
            return getAccountsPresenter.ViewModel;
        }
    }
}