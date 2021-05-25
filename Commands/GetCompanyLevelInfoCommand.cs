using FInSearchAPI.Models.Responses;
using FinSearchDataAccessLibrary.Models.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FInSearchAPI.Commands
{
    [BindProperties(SupportsGet = true)]
    public class GetCompanyLevelInfoCommand : IRequest<IEnumerable<CompanyLevelInfo>>
    { 
        public string id { get; set; } 
    }
}
