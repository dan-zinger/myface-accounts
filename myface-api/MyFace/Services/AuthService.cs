using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Helpers;
using System;

namespace Myface.Services
{
    public interface IAuthService
        {
            bool AuthenticateUser(string authorizationHeader);
            bool isUnAuthorizedResult(string authorizationHeader);
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

        public bool isUnAuthorizedResult(string authorizationHeader)
        {
            if (authorizationHeader is null)
            {
                return true;
            }
            try
            {
                var isAuthorized = AuthenticateUser(authorizationHeader);
                if (!isAuthorized)
                {
                    return true;
                }
            }
            catch (System.InvalidOperationException)
            {
                return true;
            }

            return false;
        }
    }
}