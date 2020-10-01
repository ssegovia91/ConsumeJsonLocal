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

    }
}
