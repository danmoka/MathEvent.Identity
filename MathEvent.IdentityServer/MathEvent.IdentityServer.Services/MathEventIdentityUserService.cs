using AutoMapper;
using MathEvent.IdentityServer.Constants;
using MathEvent.IdentityServer.Contracts.Repositories;
using MathEvent.IdentityServer.Contracts.Services;
using MathEvent.IdentityServer.Entities;
using MathEvent.IdentityServer.Models.Email;
using MathEvent.IdentityServer.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MathEvent.IdentityServer.Services
{
    /// <summary>
    /// Сервис по выполнению действий над пользователями
    /// </summary>
    public class MathEventIdentityUserService : IMathEventIdentityUserService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        private readonly IMapper _mapper;

        private readonly UserManager<MathEventIdentityUser> _userManager;

        private readonly IEmailService _emailService;

        public MathEventIdentityUserService(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            UserManager<MathEventIdentityUser> userManager,
            IEmailService emailService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
        }

        /// <summary>
        /// Создает пользователя
        /// </summary>
        /// <param name="createModel">Модель создания пользователя</param>
        /// <returns>Новый пользователь</returns>
        public async Task<MathEventIdentityUserReadModel> Create(MathEventIdentityUserCreateModel createModel)
        {
            var user = _mapper.Map<MathEventIdentityUser>(createModel);
            var createResult = await _repositoryWrapper.User.CreateIdentityUser(user, createModel.Password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(createResult.Errors.ToString());
            }

            await _userManager.AddToRoleAsync(user, MathEventIdentityServerRoles.User);
            await _repositoryWrapper.Save();

            var userReadModel = _mapper.Map<MathEventIdentityUserReadModel>(user);

            return userReadModel;
        }

        /// <summary>
        /// Возвращает пользователя с указанным id
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>Пользователь</returns>
        public async Task<MathEventIdentityUserReadModel> Retrieve(Guid id)
        {
            var user = await GetMathEventIdentityUser(id);

            if (user is null)
            {
                return null;
            }

            var userReadModel = _mapper.Map<MathEventIdentityUserReadModel>(user);

            return userReadModel;
        }

        /// <summary>
        /// Обновляет пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="updateModel">Модель обновления пользователя</param>
        /// <returns>Обновленный пользователь</returns>
        public async Task<MathEventIdentityUserReadModel> Update(Guid id, MathEventIdentityUserUpdateModel updateModel)
        {
            var user = await GetMathEventIdentityUser(id);

            if (user is null)
            {
                throw new InvalidOperationException($"IdentityUser with id={id} is not found");
            }

            _mapper.Map(updateModel, user);

            var updateResult = await _repositoryWrapper.User.UpdateIdentityUser(user);

            if (!updateResult.Succeeded)
            {
                throw new InvalidOperationException(updateResult.Errors.ToString());
            }

            await _repositoryWrapper.Save();

            var userReadModel = _mapper.Map<MathEventIdentityUserReadModel>(user);

            return userReadModel;
        }

        /// <summary>
        /// Удаляет пользователя с указанным id
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            var user = await GetMathEventIdentityUser(id);

            if (user is null)
            {
                throw new InvalidOperationException($"IdentityUser with id={id} is not found");
            }

            var deleteResult = await _repositoryWrapper.User.DeleteIdentityUser(user);

            if (deleteResult.Succeeded)
            {
                await _repositoryWrapper.Save();
            }
            else
            {
                throw new InvalidOperationException(deleteResult.Errors.ToString());
            }
        }

        /// <summary>
        /// Возвращает пользователем по его claims
        /// </summary>
        /// <param name="userPrincipal">Данные, определяющие пользователя</param>
        /// <returns>Пользователь</returns>
        public Task<MathEventIdentityUser> GetIdentityUser(ClaimsPrincipal userPrincipal)
        {
            return _userManager.GetUserAsync(userPrincipal);
        }

        /// <summary>
        /// Возвращает пользователя по email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Пользователь</returns>
        public Task<MathEventIdentityUser> GetIdentityUserByEmail(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }


        /// <summary>
        /// Возвращает пользователя по логину
        /// </summary>
        /// <param name="userName">Логин</param>
        /// <returns>Пользователь</returns>
        public Task<MathEventIdentityUser> GetIdentityUserByUserName(string userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        /// <summary>
        /// Возвращает список ролей пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Список ролей</returns>
        public Task<IList<string>> GetIdentityUserRoles(MathEventIdentityUser user)
        {
            return _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Проверяет, является ли пользователь в роли
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="role">Роль</param>
        /// <returns>Результат проверки</returns>
        public async Task<bool> IsInRole(Guid id, string role)
        {
            var user = await GetMathEventIdentityUser(id);

            if (user is null)
            {
                throw new InvalidOperationException($"IdentityUser with id={id} is not found");
            }

            return await _userManager.IsInRoleAsync(user, role);
        }

        /// <summary>
        /// Добавляет пользователя в роль
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="role">Роль</param>
        /// <returns>Пользователь</returns>
        public async Task<MathEventIdentityUserReadModel> AddToRole(Guid id, string role)
        {
            // TODO: валидация, что пользователь не в роли
            var user = await GetMathEventIdentityUser(id);

            if (user is null)
            {
                throw new InvalidOperationException($"IdentityUser with id={id} is not found");
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, role);

            if (addToRoleResult.Succeeded)
            {
                var userReadModel = _mapper.Map<MathEventIdentityUserReadModel>(user);

                return userReadModel;
            }
            else
            {
                // TODO: ошибки на клинте выглядят как System.List ...
                throw new InvalidOperationException(addToRoleResult.Errors.ToString());
            }
        }

        /// <summary>
        /// Удаляет пользователя из роли
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="role">Роль пользователя</param>
        /// <returns>Пользователь</returns>
        public async Task<MathEventIdentityUserReadModel> RemoveFromRole(Guid id, string role)
        {
            // TODO: валидация, что пользователь в роли
            var user = await GetMathEventIdentityUser(id);

            if (user is null)
            {
                throw new InvalidOperationException($"IdentityUser with id={id} is not found");
            }

            var removeFromRoleResult = await _userManager.RemoveFromRoleAsync(user, role);

            if (removeFromRoleResult.Succeeded)
            {
                var userReadModel = _mapper.Map<MathEventIdentityUserReadModel>(user);

                return userReadModel;
            }
            else
            {
                throw new InvalidOperationException(removeFromRoleResult.Errors.ToString());
            }
        }

        public async Task ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidOperationException("The IdentityUser's email address is empty");
            }

            var user = await GetIdentityUserByEmail(email);

            if (user is null)
            {
                return;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var message = new Message(
                new string[] { user.Email },
                "Смена пароля",
                $"Подтверждающий код: {token}");

            _emailService.SendEmail(message);
        }

        /// <summary>
        /// Меняет пароль пользователя
        /// </summary>
        /// <param name="resetModel">Модель смены пароля</param>
        /// <returns></returns>
        public async Task ResetPassword(ForgotPasswordResetModel resetModel)
        {
            var user = await GetIdentityUserByEmail(resetModel.Email);

            if (user is null)
            {
                return;
            }

            var result = await _userManager.ResetPasswordAsync(user, resetModel.Token, resetModel.Password);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.ToString());
            }

            return;
        }

        /// <summary>
        /// Возвращает пользователя с указанным id
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>Пользователь с указанным id</returns>
        private async Task<MathEventIdentityUser> GetMathEventIdentityUser(Guid id)
        {
            var user = await _repositoryWrapper.User
                .FindByCondition(user => user.Id == id)
                .SingleOrDefaultAsync();

            return user;
        }
    }
}
