using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Common.Models;

namespace Tournament.Web.BL.Facades
{
    public class MatchFacade : FacadeBase<MatchDetailModel, MatchListModel>
    {
        private readonly IMatchClient _apiClient;

        public MatchFacade(IMatchClient apiClient)
        {
            _apiClient = apiClient;
        }

        public override async Task<List<MatchListModel>> GetAllAsync()
        {
            var matchList = new List<MatchListModel>();

            var matches = await _apiClient.MatchGetAsync();
            matchList.AddRange(matches);

            return matchList;
        }

        public override async Task<MatchDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.MatchGetAsync(id);
        }

        public override async Task<Guid> CreateAsync(MatchDetailModel data)
        {
            return await _apiClient.MatchPostAsync(data);
        }

        public override async Task<Guid> UpdateAsync(MatchDetailModel data)
        {
            return await _apiClient.MatchPutAsync(data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.MatchDeleteAsync(id);
        }

        public override async Task<List<MatchListModel>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }
    }
}