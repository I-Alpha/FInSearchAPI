using FinSearchDataAccessLibrary.Models.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System; 
namespace FInSearchAPI.Handlers
{
    public class GetCompanyLevelInfoCommand : IRequest<Company>
    { 
        public readonly string id;

        public GetCompanyLevelInfoCommand(string id)
        {
            this.id = id ?? throw new ArgumentNullException(nameof(id));
        }
         
    }
}
