using FinSearchDataAccessLibrary.Models.Database;
using MediatR;
using System;
using System.Collections.Generic;

namespace FInSearchAPI.Commands
{
     
    public class GetSecurityLevelInfoCommand : IRequest<IEnumerable<Figi>>
    { 
        public readonly string id;

        public GetSecurityLevelInfoCommand(string id)
        {
            this.id = id ?? throw new ArgumentNullException(nameof(id));
        }


    }
}