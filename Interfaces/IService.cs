using FinSearchDataAccessLibrary.Models.Database; 
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FInSearchAPI.Interfaces
{
    public interface IService <T>
    {
        ILogger Logger { get; set; }
        public string Type { get; set; }
        Task<string> GetEntryAsync(string id) ;
        Task<string> MatchAsync(Company company); 
        Task<string> SearchAsync(string queryString);
    }
}