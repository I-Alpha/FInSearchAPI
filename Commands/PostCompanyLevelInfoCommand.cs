using FinSearchDataAccessLibrary.Models.Database;
using MediatR;
using System;
using System.Collections.Generic;

namespace FInSearchAPI.Commands
{
    public class PostCompanyLevelInfoCommand : IRequest<IEnumerable<Company>>
    {
        public readonly string companyjson;

        public PostCompanyLevelInfoCommand(string _companyjson)
        {
            companyjson = _companyjson ?? throw new ArgumentNullException(nameof(_companyjson));
        }
    }
}