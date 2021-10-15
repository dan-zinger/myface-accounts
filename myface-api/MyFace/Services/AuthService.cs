using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Helpers;
using System;
using MyFace.Models.Database;


namespace Myface.Services
{
    public interface IAuthService
        {
            bool AuthenticateUser(string authorizationHeader);
            bool IsAuthenticated(string authorizationHeader);
            bool isAuthorizedAdmin(User user);
        }
    public class AuthService : IAuthService
    {
        private readonly IUsersRepo _users;
        public AuthService(IUsersRepo users)
        {
            _users = users;
        }
        public bool AuthenticateUser(string authorizationHeader)
        {
            var parts = authorizationHeader.Split(' ');
            var userNamePassword = parts[1];
            var userNamePasswordDecoded = AuthHelper.Base64Decode(userNamePassword);
            var userNamePasswordDecodedSplit = userNamePasswordDecoded.Split(':');
            var userName = userNamePasswordDecodedSplit[0];
            var passwordToCheck = userNamePasswordDecodedSplit[1];

            var user = _users.GetByUserName(userName);
            var hashed_password = user.hashed_password;
            byte[] salt = user.salt;

            var hashedPasswordToCheck = AuthHelper.GetHashedPassword(salt, passwordToCheck);
            if (hashed_password == hashedPasswordToCheck)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool IsAuthenticated(string authorizationHeader)
        {
            if (authorizationHeader is null)
            {
                return false;
            }
            try
            {
                var isAuthorized = AuthenticateUser(authorizationHeader);
                if (!isAuthorized)
                {
                    return false;
                }
            }
            catch (System.InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        public bool isAuthorizedAdmin(User user)
        {
            if (user.Role == RoleType.ADMIN)
                {
                    return true;
                }
            
            return false;
        }
    }
}