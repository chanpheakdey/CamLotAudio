using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using ACEAppAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EOTAudio.Controllers
{
    [Route("api/[controller]")]
    public class AudioController : ControllerBase
    {

        [HttpGet]
        public async Task<dynamic> Get(int id,string name,bool streamBinary)
        {
            try
            {
                bool ParametersAreValid = (id != 0 && ( name != null && name != string.Empty));
                if (ParametersAreValid)
                {
                    if (!streamBinary)
                    {
                        FileStream fs = System.IO.File.Open($"Audio/{name}.mp3", FileMode.Open, FileAccess.Read, FileShare.Read);
                        //return File(fs, "audio/mp3");
                        FileStreamResult fsresult;
                        fsresult = new FileStreamResult(fs, "audio/wav");

                        return fsresult;
                    }
                    else
                    {
                        DB EOT = new DB(Connection.GetConnectionString("EOT"));
                        string query = $"SELECT AudioData FROM tblAudio WHERE AudioID = @AudioID";
                        List<Dictionary<string, dynamic>> queryParameters = new List<Dictionary<string, dynamic>>() {
                            new Dictionary<string, dynamic>(){ {"paramName", "@AudioID" },{"paramType",SqlDbType.Int},{"paramValue",id} }
                        };
                        DataTable result = await EOT.ExecuteSafeQueryForResult(query, queryParameters);
                        byte[] base64 = (byte[])result.Rows[0]["AudioData"];
                        return File(base64, "audio/mp3");
                    }
                }
                else
                {
                    return "parameters missing !";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Internal server error !";
            }
        }

        [Route("UpdateTable")]
        [HttpPut]
        public async Task<dynamic> UpdateTable(int id,IFormCollection data)
        {
            IFormFile file = data.Files["file"];
            Stream stream = file.OpenReadStream();
            byte[] bytes = new byte[file.Length];
            await stream.ReadAsync(bytes, 0, (int)file.Length);
            DB EOT = new DB(Connection.GetConnectionString("EOT"));
            string finalQuery = $"UPDATE tblAudio SET AudioData = @Binary WHERE AudioID = @AudioID";
            List<Dictionary<string, dynamic>> queryParameters = new List<Dictionary<string, dynamic>>(){
                new Dictionary<string, dynamic>(){ {"paramName", "@AudioID" },{"paramType",SqlDbType.Int},{"paramValue", id } },
                new Dictionary<string, dynamic>(){ {"paramName", "@Binary" },{"paramType",SqlDbType.VarBinary},{"paramValue",bytes} }
            };
            return await EOT.ExecuteSafeQuery(finalQuery, queryParameters);
        }
    }
}
