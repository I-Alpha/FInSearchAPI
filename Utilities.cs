
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAcessLibrary.DataAccess; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq; 
using System.Reflection; 
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;

namespace FinSearchDataAPI
{
    public static class Utilities
    {
        static ILogger Logger = new LoggerFactory().CreateLogger("Utilies");
         
        public static bool DatabaseISeeded { get; private set; }

        public static string Serialize(this object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(metaToken);

        }

        public static JObject Deserialize(this object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }
            return JObject.FromObject(metaToken);

        }

        public static IDictionary<string, string> ToKeyValue(this object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            JToken token = metaToken as JToken;
            if (token == null)
            {
                return ToKeyValue(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = child.ToKeyValue();
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                                                 .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            var value = jValue?.Type == JTokenType.Date ?
                            jValue?.ToString("o", CultureInfo.InvariantCulture) :
                            jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }




        /// <summary>
        /// Takes the full name of a resource and loads it in to a stream.
        /// </summary>
        /// <param name="resourceName">Assuming an embedded resource is a file
        /// called info.png and is located in a folder called Resources, it
        /// will be compiled in to the assembly with this fully qualified
        /// name: Full.Assembly.Name.Resources.info.png. That is the string
        /// that you should pass to this method.</param>
        /// <returns></returns>
        public static Stream GetEmbeddedResourceStream(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        }

        /// <summary>
        /// Get the list of all emdedded resources in the assembly.
        /// </summary>
        /// <returns>An array of fully qualified resource names</returns>
        public static string[] GetEmbeddedResourceNames()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames();
        }


        public static void SeedLookUpData(string path, FinSearchDBContext FinSearchDbContext)
        {

            if (DatabaseISeeded)
                return;
            //Should only be used on creation of database from local file.

            string json = GetCSVJson(path);

            // and add to db
            AddLookUpDataFromJson(json, FinSearchDbContext);
            DatabaseISeeded = true;
        }

        private static string GetCSVJson(string path)
        {
            //For loading csv lookupTable data at the creation of database
            var g = GetDataTableFromCSVFile(path);

            // serialize datatable
            string json = JsonConvert.SerializeObject(g, Formatting.Indented);
            return json;
        }

        public static void AddLookUpDataFromJson(string json, FinSearchDBContext context)
        {
            List<LookUpRow> dataCollection = GetLookUpRowCollection(json);
            using (context)
            {
                context.BloomBergLookUp.AddRange(dataCollection);
                context.SaveChanges();
            }
            context.DisposeAsync();  
        }
         
        public static List<LookUpRow> GetExcelData(string path)
            {
                return GetLookUpRowCollection(GetCSVJson(path));
            }


    private static List<LookUpRow> GetLookUpRowCollection(string json)
        {
            /// Serialize json Add look up data to database.  
            return (JsonConvert.DeserializeObject<List<LookUpRow>>(json)).Where(x => x.CORPEXCHANGE != null).ToList();
        }

        private static DataTable GetDataTableFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();

            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();

                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }

                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return csvData;
        }



    }
  
} 