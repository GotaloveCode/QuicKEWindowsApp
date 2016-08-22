using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Mfundi.Client
{
    public class GetProfessionServiceProxy : ServiceProxy, IGetProfessionServiceProxy
    {
        public GetProfessionServiceProxy()
            : base("GetReportsByUser")
        {
        }

        public async Task<GetReportsByUserResult> GetReportsByUserAsync()
        {
            var input = new JsonObject();
            var executeResult = await this.ExecuteAsync(input); 

            // did it work?
            if (!(executeResult.HasErrors))
            {
                // get the reports...
                string asString = executeResult.Output.GetNamedString("reports");

                // create some objects...
                var reports = JsonConvert.DeserializeObject<List<ProfessionItem>>(asString);

                // return...
                return new GetReportsByUserResult(reports);
            }
            else
                return new GetReportsByUserResult(executeResult);
        }
    }
}
