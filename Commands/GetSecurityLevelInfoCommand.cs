using FinSearchDataAccessLibrary.Models.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FInSearchAPI.Commands
{

    [BindProperties(SupportsGet = true)]
    public class GetSecurityLevelInfoCommand : IRequest<IEnumerable<FigiInstrument>>
    { 
        public readonly string id;

        public GetSecurityLevelInfoCommand(string Id)
        {
            this.id = Id ?? throw new ArgumentNullException(nameof(Id));
        }


    }
}