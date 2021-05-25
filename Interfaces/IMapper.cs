using FinSearchDataAccessLibrary.Models.Database;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FInSearchAPI.Interfaces
{
    public interface IMapper
    {
        ILogger Logger { get; set; }

        string MapMicToCompositeCode(string _mic);
        Task<string> MapTickerToFigi(string ticker);
        Task<Company> PopulateInfoForCompany(Company _company);
    }
}