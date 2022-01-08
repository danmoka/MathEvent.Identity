using AutoMapper;
using MathEvent.IdentityServer.Authorization;
using MathEvent.IdentityServer.Contracts.Services;
using MathEvent.IdentityServer.Contracts.Validation.User;
using MathEvent.IdentityServer.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MathEventIdentityUsersController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IMathEventIdentityUserService _mathEventIdentityUserService;

        private readonly IAuthorizationService _authorizationService;

        private readonly IMathEventIdentityUserCreateModelValidator _mathEventIdentityUserCreateModelValidator;

        private readonly IMathEventIdentityUserUpdateModelValidator _mathEventIdentityUserUpdateModelValidator;

        private readonly IForgotPasswordModelValidator _forgotPasswordModelValidator;

        private readonly IForgotPasswordResetModelValidator _forgotPasswordResetModelValidator;

        private readonly IMathEventIdentityUserRoleModelValidator _mathEventIdentityUserRoleModelValidator;

        public MathEventIdentityUsersController(
            IMapper mapper,
            IMathEventIdentityUserService userService,
            IAuthorizationService authorizationService,
            IMathEventIdentityUserCreateModelValidator userCreateModelValidator,
            IMathEventIdentityUserUpdateModelValidator userUpdateModelValidator,
            IForgotPasswordModelValidator forgotPasswordModelValidator,
            IForgotPasswordResetModelValidator forgotPasswordResetModelValidator,
            IMathEventIdentityUserRoleModelValidator mathEventIdentityUserRoleModelValidator)
        {
            _mapper = mapper;
            _mathEventIdentityUserService = userService;
            _authorizationService = authorizationService;
            _mathEventIdentityUserCreateModelValidator = userCreateModelValidator;
            _mathEventIdentityUserUpdateModelValidator = userUpdateModelValidator;
            _forgotPasswordModelValidator = forgotPasswordModelValidator;
            _forgotPasswordResetModelValidator = forgotPasswordResetModelValidator;
            _mathEventIdentityUserRoleModelValidator = mathEventIdentityUserRoleModelValidator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Create([FromBody] MathEventIdentityUserCreateModel userCreateModel)
        {
            var validationResult = await _mathEventIdentityUserCreateModelValidator.Validate(userCreateModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdUser = await _mathEventIdentityUserService.Create(userCreateModel);

            if (createdUser is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка во время создания пользователя");
            }

            return CreatedAtAction(nameof(Retrieve), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MathEventIdentityUserReadModel>> Retrieve([FromRoute] Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest($"id не задан");
            }

            var user = await _mathEventIdentityUserService.Retrieve(id);

            if (user is null)
            {
                return NotFound($"Пользователь с id={id} не найден");
            }

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, user, Operations.Read);

            if (!authorizationResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status403Forbidden, $"Вам нельзя получить информацию о пользователе с id={id}");
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] MathEventIdentityUserUpdateModel userUpdateModel)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("id не задан");
            }

            var validationResult = await _mathEventIdentityUserUpdateModelValidator.Validate(userUpdateModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _mathEventIdentityUserService.Retrieve(id);

            if (user is null)
            {
                return NotFound($"Пользователь с id={id} не найден");
            }

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, user, Operations.Update);

            if (!authorizationResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status403Forbidden, $"Вам нельзя редактировать пользователя с id={id}");
            }

            var updatedUser = await _mathEventIdentityUserService.Update(id, userUpdateModel);

            if (updatedUser is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка во время обновления пользователя");
            }

            return Ok(updatedUser);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdate([FromRoute] Guid id, [FromBody] JsonPatchDocument<MathEventIdentityUserUpdateModel> patchDocument)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("id не задан");
            }

            if (patchDocument is null)
            {
                return BadRequest("Тело запроса не задано");
            }

            var user = await _mathEventIdentityUserService.Retrieve(id);

            if (user is null)
            {
                return NotFound($"Пользователь с id={id} не найден");
            }

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, user, Operations.Update);

            if (!authorizationResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status403Forbidden, $"Вам нельзя редактировать пользователя с id={id}");
            };

            var userToPatch = _mapper.Map<MathEventIdentityUserUpdateModel>(user);
            patchDocument.ApplyTo(userToPatch, ModelState);

            var validationResult = await _mathEventIdentityUserUpdateModelValidator.Validate(userToPatch);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updatedUser = await _mathEventIdentityUserService.Update(id, userToPatch);

            if (updatedUser is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка во время обновления пользователя");
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Destroy([FromRoute] Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("id не задан");
            }

            var user = await _mathEventIdentityUserService.Retrieve(id);

            if (user is null)
            {
                return NotFound($"Пользователь с id={id} не найден");
            }

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, user, Operations.Delete);

            if (!authorizationResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status403Forbidden, $"Вам нельзя удалять пользователя с id={id}");
            }

            await _mathEventIdentityUserService.Delete(id);

            return NoContent();
        }

        [HttpPost("ForgotPassword/")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
        {
            var validationResult = await _forgotPasswordModelValidator.Validate(forgotPasswordModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _mathEventIdentityUserService.ForgotPassword(forgotPasswordModel.Email);

            return Ok();
        }

        [HttpPost("ResetPassword/")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ForgotPasswordResetModel resetModel)
        {
            var validationResult = await _forgotPasswordResetModelValidator.Validate(resetModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _mathEventIdentityUserService.ResetPassword(resetModel);

            return Ok();
        }

        [HttpPost("AddToRole/")]
        public async Task<IActionResult> AddToRole([FromBody] MathEventIdentityUserRoleModel userRoleModel)
        {
            var validationResult = await _mathEventIdentityUserRoleModelValidator.Validate(userRoleModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, userRoleModel, Operations.Create);

            if (!authorizationResult.Succeeded)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    $"Вам нельзя добавлять пользователя с id={userRoleModel.Id} в роль={userRoleModel.Role}");
            }

            var user = await _mathEventIdentityUserService.AddToRole(userRoleModel.Id, userRoleModel.Role);

            if (user is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка во время добавления в роль");
            }

            return Ok(user);
        }

        [HttpPost("RemoveFromRole/")]
        public async Task<IActionResult> RemoveFromRole([FromBody] MathEventIdentityUserRoleModel userRoleModel)
        {
            var validationResult = await _mathEventIdentityUserRoleModelValidator.Validate(userRoleModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, userRoleModel, Operations.Delete);

            if (!authorizationResult.Succeeded)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    $"Вам нельзя удалять пользователя с id={userRoleModel.Id} из роли={userRoleModel.Role}");
            }

            var user = await _mathEventIdentityUserService.RemoveFromRole(userRoleModel.Id, userRoleModel.Role);

            if (user is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка во время удаления из роли");
            }

            return Ok(user);
        }
    }
}
