using FinSearchDataAccessLibrary.Models.Database;
using System.Collections.Generic;

namespace FInSearchAPI.Interfaces
{
    public interface IArrayResponse
    {
        List<Figi> Data { get; set; }
        string Error { get; set; }
    }
}