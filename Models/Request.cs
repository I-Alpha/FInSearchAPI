using FInSearchAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FInSearchAPI.Models
{
    public abstract class Request : IRequest
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Source { get ; set ; }
        public string BaseUrl { get; set; }
        public string Parameters { get; set ; }
 
    }
}
