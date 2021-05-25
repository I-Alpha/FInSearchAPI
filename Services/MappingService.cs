using System.Threading.Tasks; 
using FinSearchDataAccessLibrary.Models.Database; 
using System;   
using System.Data; 
using System.Collections.Generic; 
using System.Linq;
using FinSearchDataAcessLibrary.DataAccess;
using FInSearchAPI.Models.Requests;
using FInSearchAPI.Interfaces; 
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using FinSearchDataAccessLibrary;
using FinSearchDataAPI;

namespace FInSearchAPI.Services
{
    public class MappingService :  IMapper  
    {

        #region Fields

        private readonly OpenFigiService OpenFigiService;
        private readonly PermIDService PermIDService;
        private readonly FinSearchDBContext FinSearchDbContext;
        private readonly NewtonsoftJsonSerializer NewtonsoftJsonSerializer = new NewtonsoftJsonSerializer();
        public  ILogger Logger { get; set; }

        #endregion

        #region Constructors
        public MappingService(OpenFigiService _OpenFigiService, PermIDService _PermIDService , FinSearchDBContext _context, ILogger<MappingService> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            OpenFigiService = _OpenFigiService ?? throw new ArgumentNullException(nameof(_OpenFigiService));
            PermIDService = _PermIDService ?? throw new ArgumentNullException(nameof(_PermIDService));
            FinSearchDbContext = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        public MappingService(FinSearchDBContext _context)
        {
            FinSearchDbContext = _context;
        }
        #endregion

        #region Methods
        public  async Task<Company> PopulateInfoForCompany(Company _company) {

            if (!_company.IsComplete())
            {
                var entityjson = await PermIDService.GetEntryAsync(_company.PermId);

                _company = (Company)Service.CreateObjectFromJson(entityjson);

                if (_company == null)
                {
                    return new Company(); 
                }
/*
                //employees 
                var employeesstr = await PermIDService.GetEmployeesForCompanyAsync(_company);
*/
             /*   _company.Employees =*/  
                //quotes
                _company.PrimaryQuote = (Quote)await PermIDService.GetPrimaryQuoteAsync(_company);

                if (_company.PrimaryQuote != null)
                    await FinSearchDbContext.Quotes.AddAsync(_company.PrimaryQuote);

                //figi 
                _company.OpenFigiEntry = await MapCompanyToFigi(_company);
                if (_company.OpenFigiEntry != null)
                   _company.CompositeCode = MapMicToCompositeCode(_company.PrimaryQuote.MIC);
                 
            }
            return _company.fillTicker(); 
       
        } 
         
        public string MapMicToCompositeCode(string _mic) {             
       
                var qry = from LookUpRow in FinSearchDbContext.BloomBergLookUp
                          where LookUpRow.MIC == _mic
                          select   LookUpRow.CompositeCode ;
                var q=  qry.FirstOrDefault();                    
                return q;
            
        }

        public async Task<string> MapTickerToFigi(string ticker)
        {
            var temp = new List<FigiRequest> { new FigiRequest(ticker) };
            var json = temp.Serialize() ?? throw new ArgumentNullException(nameof(temp));
            var response = await OpenFigiService.SearchAsync(json) ?? throw new ArgumentNullException(nameof(json));
            return response;
        }


        private async Task<FigiInstrument> MapCompanyToFigi(Company _Company)
        {
            if (FinSearchDbContext.Figi.Any(x => x.Ticker == _Company.PrimaryQuote.Ticker))
                return FinSearchDbContext.Figi.Where(x => x.Ticker == _Company.PrimaryQuote.Ticker).FirstOrDefault();
            //Company to figi obj for post request
            var FigiRequestObjjson = NewtonsoftJsonSerializer.Serialize(new List<FigiRequest> {   OpenFigiService.ConvertCompanyToFigiRequest(_Company) });
 
            var results =   JsonConvert.DeserializeObject<List<FigiInstrument>>( await OpenFigiService.SearchAsync(FigiRequestObjjson));

            if (results != null)
            {
                if (results.Count != 0)
                {
                    {
                        await FinSearchDbContext.Figi.AddAsync(results[0]);
                        FinSearchDbContext.SaveChanges();
                        return await MapCompanyToFigi(_Company);
                    }
                }
            }
            //
            return new FigiInstrument();
        }
        #endregion
    }

}