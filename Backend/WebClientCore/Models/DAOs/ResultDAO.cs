using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebClientCore.Models.DAOs
{
    public class ResultDAO
    {
        public ResultDAO()
        {
        }

        public async Task<Result> GetResult(int id)
        {
            var client = RequestHelper.GetHttpClient();
            using (var response = await client.GetAsync("api/document/" + id + "/result"))
            {
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Result>(body);
                
                if(result.PeerCheckResult != null){
                    Console.WriteLine(result.PeerCheckResult.Details.Count);
                    MergePosition(result.PeerCheckResult);
                }
                if(result.WebCheckResult != null){
                    MergePosition(result.WebCheckResult);
                }
                return result;
            }
        }

        private void MergePosition(ResultDetail result)
        {
            StringBuilder baseMethod = new StringBuilder();
            StringBuilder simMethod = new StringBuilder();
            List<PositionDetail> sourcePositions = new List<PositionDetail>();
            List<PositionDetail> simPositions = new List<PositionDetail>();

            int startPosition = 0;
            foreach (var data in result.Details)
            {
                if (data.SimRatio > 0)
                {
                    var baseLines = new List<String>(data.BaseMethod.Split("\n"));
                    var simLines = new List<String>(data.SimMethod.Split("\n"));

                    //Compare 2 method lenght
                    while (baseLines.Count < simLines.Count) { baseLines.Add(""); }
                    while (simLines.Count < baseLines.Count) { simLines.Add(""); }

                    
                    baseLines.Add("");
                    simLines.Add("");

                    baseLines.ForEach(l => baseMethod.AppendLine(l));
                    simLines.ForEach(l => simMethod.AppendLine(l));
                    
                    data.Position.SourcePositions.ForEach(l => { l.StartLine += startPosition + 1; l.EndLine += startPosition + 1; });
                    data.Position.SimPositions.ForEach(l => { l.StartLine += startPosition + 1; l.EndLine += startPosition + 1; });

                    sourcePositions.AddRange(data.Position.SourcePositions);
                    simPositions.AddRange(data.Position.SimPositions);

                    Console.WriteLine("" + JsonConvert.SerializeObject(data.Position));

                    startPosition += baseLines.Count;
                }
                else
                {
                    var baseLines = new List<String>(data.BaseMethod.Split("\n"));
                    var simLines = new List<String>();

                    
                    while (baseLines.Count < simLines.Count) { baseLines.Add(""); }
                    while (simLines.Count < baseLines.Count) { simLines.Add(""); }

                    baseLines.Add("");
                    simLines.Add("");

                    baseLines.ForEach(l => baseMethod.AppendLine(l));
                    simLines.ForEach(l => simMethod.AppendLine(l));
                    startPosition += baseLines.Count;
                }
            }
            result.MergeDetail = new MergeDetail
            {
                BaseMethod = baseMethod.ToString(),
                SimMethod = simMethod.ToString(),
                SourcePositions = sourcePositions,
                SimPositions = simPositions
            };
        }
    }
}
