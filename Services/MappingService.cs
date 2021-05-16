using System.IO;
using System.Threading.Tasks; 
using FinSearchDataAccessLibrary.Models.Database; 
using System;   
using Microsoft.VisualBasic.FileIO;
using System.Data; 
using System.Collections.Generic; 
using Newtonsoft.Json; 
using System.Linq;
using FinSearchDataAcessLibrary.DataAccess;
using FinSearchDataAccessLibrary;
using FinSearchDataAccessLibrary.Interfaces;
using FinSearchDataAccessLibrary.Handlers;
using FInSearchAPI.Models.Requests;
using FInSearchAPI.Models;

namespace FInSearchAPI.Services
{
    public class MappingService : Service
    {
        private readonly OpenFigiService OpenFigiService;
        private readonly PermIDService PermIDService;
        private readonly FinSearchDBContext FinSearchDbContext; 

        public MappingService(OpenFigiService _OpenFigiService, PermIDService _PermIDService , FinSearchDBContext _context)
        {
            OpenFigiService = _OpenFigiService ?? throw new ArgumentNullException(nameof(_OpenFigiService));
            PermIDService = _PermIDService ?? throw new ArgumentNullException(nameof(_PermIDService));
            FinSearchDbContext = _context ?? throw new ArgumentNullException(nameof(_context));
        }
         
        public MappingService(FinSearchDBContext _context)
        {
            FinSearchDbContext = _context;
        }

        public async Task<Company> GetInfoForEntry(Company _company) {


            if (!_company.IsComplete())
            {
                var entityjson=  PermIDService.LookUpByIdAsync(_company.PermId).Result;
                _company  =  (Company)CreateObjectFromJson(entityjson)  ;
                _company.PrimaryQuote = (Quote)await PermIDService.GetQuoteAsync(_company);
                _company.OpenFigiEntry = GetFigiForCompanyAsync(_company).Result; 
                _company.CompositeCode = await GetCompositCodeAsync(_company.PrimaryQuote.MIC);               
            }

            return _company; 
       
        } 
         
        public async Task<string> GetCompositCodeAsync(string _mic) {
             
            using (FinSearchDbContext) 
            {
                var qry = from LookUpRow in FinSearchDbContext.BloomBergLookUp
                          where LookUpRow.MIC == _mic
                          select   LookUpRow.CompositeCode ;
                var q=  qry.FirstOrDefault();                    
                return q;
            } 
        }

        public async Task<Figi> GetFigiForCompanyAsync(Company _Company)
        {
            //Company to figi obj for post request
            var FigiRequestObj = new List<FigiRequest> { OpenFigiService.ConvertCompanyToFigiRequest(_Company).Result };
            var list = new List<FigiRequest>()
            {
                new FigiRequest("TICKER", "MSFT").WithExchangeCode("US").WithMarketSectorDescription("Equity")
            };

            var response = await OpenFigiService.MapAsync(FigiRequestObj);

            foreach (var dataInstrument in response.Data)
                if (dataInstrument.Data != null && dataInstrument.Data.Any())
                    foreach (var instrument in dataInstrument.Data)
                        return instrument;


        /*                if (response != null)
            {

                var result = (Figi)((Interfaces.IService)OpenFigiService).CreateObjectFromJson(response);

          *//*      _Company.OpenFigiEntry = result;
*//*
                return result;*/
             

            //
            return new Figi();
        }
          
      

 

    

   
    }
     
}