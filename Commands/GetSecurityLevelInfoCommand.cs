using FinSearchDataAccessLibrary.Models.Database;
using MediatR;
using System;

namespace FInSearchAPI.Controllers.Commands
{
     
    public class GetSecurityLevelInfoCommand : IRequest<Figi>
    { 
        public readonly string id;

        public GetSecurityLevelInfoCommand(string id)
        {
            this.id = id ?? throw new ArgumentNullException(nameof(id));
        }


    }
}