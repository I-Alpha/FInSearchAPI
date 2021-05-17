using FinSearchDataAccessLibrary.Models.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FInSearchAPI.Commands
{
    [BindProperties(SupportsGet = true)]
    public class GetCompanyLevelInfoCommand : IRequest<IEnumerable<Company>>
    { 
        public string id { get; set; }
        public GetCompanyLevelInfoCommand()
        {
         
        }
         
    }
}
