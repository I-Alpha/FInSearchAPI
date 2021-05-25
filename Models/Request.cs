using FInSearchAPI.Interfaces;
using MediatR; 

namespace FInSearchAPI.Models
{
    public abstract class Request : IRequest
    {
        #region Fields
        public int Id { get; set; }
        public string Type { get; set; }
        public string Source { get ; set ; }
        public string BaseUrl { get; set; }
        public string Parameters { get; set ; }

        #endregion

    }
}
