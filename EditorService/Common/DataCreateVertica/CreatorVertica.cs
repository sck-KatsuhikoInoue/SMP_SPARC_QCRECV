using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EditorService
{
    public class CreatorVertica
    {
        private DbAccessServiceVertica _dbAccessService;
        private JsonSerializerOptions _jsonSerializerOptions;

        public CreatorVertica()
        {
            var option = VerticaDbOption.FromConfig();
            var db = new VerticaDb(option);
            _dbAccessService = new DbAccessServiceVertica(db);
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            };
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public string GetConnectionString() => _dbAccessService.GetConnectionString();

        public async Task<IEnumerable<SpcMasterResult>> SpcChartSearch(SpcMasterParameter param)
        {
            return await _dbAccessService.SpcChartSearch(param);
        }
    }
}
