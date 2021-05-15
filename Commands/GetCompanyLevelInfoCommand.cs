using FinSearchDataAccessLibrary.Models.Database;
using MediatR; 
using System;
using System.Collections.Generic;

namespace FInSearchAPI.Commands
{
    public class GetCompanyLevelInfoCommand : IRequest<IEnumerable<Company>>
    { 
        public readonly string id;

        public GetCompanyLevelInfoCommand(string _id)
        {
            this.id = _id ?? throw new ArgumentNullException(nameof(_id));
        }
         
    }
}
