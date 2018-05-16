using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHibernate;
using SteamWeb.Infrastructure;
using SteamWeb.Infrastructure.Authentication;


namespace SteamWeb.ViewComponents.NavBar
{
    public class NavBar
    {
        public class Query : IRequest<Model>
        {

        }

        public class Model
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public decimal Wallet { get; set; }
            public bool IsAuthenticated { get; set; }
            public bool IsAdmin { get; set; }
        }

        /*
        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly ISession _session;
            private readonly ICurrentUserContext _context;
            public Handler(ISession session, ICurrentUserContext context)
            {
                _session = session;
                _context = context;
            }

            public Model Handle(Query message, CancellationToken token)
            {
                if (!_context.IsAuthenticated)
                {
                    return new Model
                    {
                        IsAuthenticated = false
                    };
                }

                return new Model
                {
                    IsAuthenticated = true,
                    Username = _context.UserName,
                    IsAdmin = _context.UserType
                };
            }
        }
        */
    }
}
