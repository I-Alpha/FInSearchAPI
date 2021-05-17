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
                var entityjson = await PermIDService.LookUpByIdAsync(_company.PermId);

                _company = (Company)CreateObjectFromJson(entityjson);

                if (_company != null)
                {
                    await FinSearchDbContext.Companies.AddAsync(_company);
                }
                else return _company;
                

                //quotes
                _company.PrimaryQuote = (Quote)await PermIDService.GetQuoteAsync(_company);

                if (_company.PrimaryQuote != null)
                    await FinSearchDbContext.Quotes.AddAsync(_company.PrimaryQuote);

                //figi
                _company.OpenFigiEntry = await GetFigiForCompanyAsync(_company);
                if (_company.OpenFigiEntry != null)
                    await FinSearchDbContext.Figi.AddAsync(_company.OpenFigiEntry);

                _company.CompositeCode = await GetCompositCodeAsync(_company.PrimaryQuote.MIC);

                await FinSearchDbContext.SaveChangesAsync();
            }
            return _company; 
       
        } 
         
        public async Task<string> GetCompositCodeAsync(string _mic) {
             
       
                var qry = from LookUpRow in FinSearchDbContext.BloomBergLookUp
                          where LookUpRow.MIC == _mic
                          select   LookUpRow.CompositeCode ;
                var q=  qry.FirstOrDefault();                    
                return q;
            
        } 

        public async Task<FigiInstrument> GetFigiForCompanyAsync(Company _Company)
        {
            //Company to figi obj for post request
            var FigiRequestObj = new List<FigiRequest> { OpenFigiService.ConvertCompanyToFigiRequest(_Company).Result };
 
            var datalist = await OpenFigiService.MapAsync(FigiRequestObj);

            if (datalist != null)
            {

                return datalist[0];
            }

            //
            return new FigiInstrument();
        }
          
      

 

    

   
    }
     
}