using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumeJsonLocal.Utils
{
    public class DataHandler
    {
        readonly IWebHostEnvironment _hostingEnvironment;
        public DataHandler(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<List<T>> LoadJsonFile<T>(string fileName)
        {
            var path = $"{_hostingEnvironment?.ContentRootPath}\\Data\\{fileName}";

            if (File.Exists(path))
            {
                using (var r = new StreamReader(path))
                {
                    var json = await r.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
            }
            return default(List<T>);
        }

        public async Task<string> AddJsonRecordInFile<T>(string fileName, T model ,string id)
        {
            var path = $"{_hostingEnvironment?.ContentRootPath}\\Data\\{fileName}";
            string newJson;

            if (File.Exists(path))
            {
                try
                {
                    //Serialize model to json object
                    newJson = JsonConvert.SerializeObject(model);

                    //clean actual content
                    await File.WriteAllTextAsync(path, string.Empty);
                    //write new content
                    var foo = File.WriteAllTextAsync(path, newJson);

                    return id;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                    return string.Empty;
                }
                
            }
            
            return string.Empty;
        }

        public async Task<V> UpdateJsonRecordInFile<T, V>(string fileName, T model, V modelToReturn)
        {
            var path = $"{_hostingEnvironment?.ContentRootPath}\\Data\\{fileName}";
            string newJson;

            if (File.Exists(path))
            {
                try
                {
                    newJson = JsonConvert.SerializeObject(model);

                    //clean actual content
                    await File.WriteAllTextAsync(path, string.Empty);
                    //write new content
                    var foo = File.WriteAllTextAsync(path, newJson);

                    return modelToReturn;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                    return default(V);
                }

            }

            return default(V);
        }

        public async Task<bool> DeleteJsonRecordInFile<T>(string fileName, T model)
        {
            var path = $"{_hostingEnvironment?.ContentRootPath}\\Data\\{fileName}";
            string newJson;

            if (File.Exists(path))
            {
                try
                {
                    newJson = JsonConvert.SerializeObject(model);

                    //clean actual content
                    await File.WriteAllTextAsync(path, string.Empty);
                    //write new content
                    var foo = File.WriteAllTextAsync(path, newJson);

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                    return false;
                }

            }

            return false;
        }

    }
}
