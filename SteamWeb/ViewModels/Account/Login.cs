using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NHibernate;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using SteamWeb.Models;
using SteamWeb.Infrastructure.Authentication;
using System.Threading;

namespace SteamWeb.ViewModels.Account
{
    public class Login
    {
        [Required]
        public virtual string Username { get; set; }

        [Required]
        public virtual string Password { get; set; }

        public virtual bool Admin { get; set; }

        public class Command : IRequest
        {
            public int UserId { get; set; }
        }
        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly ISession _nhsession;
            private readonly IAuthenticationManager _authManager;
            public CommandHandler(ISession session, IAuthenticationManager authManager)
            {
                _nhsession = session;
                _authManager = authManager;
            }

            public void Handle(Command message)
            {
                var user = _nhsession.Get<User>(message.UserId);

                var userInformations = new List<UserInfo>
                {
                    new UserNameInfo(user.Username),
                    new UserIdInfo(user.Id),
                    new UserTypeInfo(user.IsAdmin),
                    new WorkingDateInfo()
                };
                _authManager.SignInAsync(userInformations);

                /* TODO: Look into detecting whether or not a user has enabled
                 * cookies. If they are disabled, fall back to storing this 
                 * "session" data in the database instead???
                 */
            }

            public Task Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
