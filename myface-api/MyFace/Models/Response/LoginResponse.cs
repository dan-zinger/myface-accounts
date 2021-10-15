using System;
using MyFace.Models.Database;

namespace MyFace.Models.Response
{
    public class LoginResponse
    {

        private readonly User _user;

        public LoginResponse (User user)
        {
            _user = user;
        }

        public RoleType Role =>  _user.Role;
        public int UserId => _user.Id;
    }
}