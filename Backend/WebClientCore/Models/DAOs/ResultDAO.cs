using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebClient.Models.DAOs
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

                    while (baseLines.Count < simLines.Count) { baseLines.Add(""); }
                    while (simLines.Count < baseLines.Count) { simLines.Add(""); }

                    baseLines.Add("");
                    simLines.Add("");

                    baseLines.ForEach(l => baseMethod.AppendLine(l.Trim()));
                    simLines.ForEach(l => simMethod.AppendLine(l.Trim()));

                    data.Position.SourcePositions.ForEach(l => { l.StartLine += startPosition; l.EndLine += startPosition; });
                    data.Position.SimPositions.ForEach(l => { l.StartLine += startPosition; l.EndLine += startPosition; });

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

                    baseLines.ForEach(l => baseMethod.AppendLine(l.Trim()));
                    simLines.ForEach(l => simMethod.AppendLine(l.Trim()));
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
