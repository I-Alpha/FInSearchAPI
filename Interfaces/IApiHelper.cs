using RestSharp;
using System.Net.Http;

namespace FInSearchAPI.Interfaces
{
    public interface IApiHelper
    {
        RestClient RestClient { get; }
        HttpClient HttpClient { get; }
    }
}