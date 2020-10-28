using System;
using System.Collections.Generic;
using HotChocolate;

namespace src.Api.Models
{
    public class CurrentUser
    {
        public string UserId { get; }

        public CurrentUser(string userId)
        {
            UserId = userId;
        }
    }

    public class CurrentUserGlobalState : GlobalStateAttribute
    {
        public CurrentUserGlobalState() : base("currentUser")
        {
        }
    }
}