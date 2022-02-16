using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Common.Models;

namespace Tournament.Web.BL.Facades
{
    public class TeamFacade : FacadeBase<TeamDetailModel, TeamListModel>
    {
        private readonly ITeamClient _apiClient;

        public TeamFacade(ITeamClient apiClient)
        {
            _apiClient = apiClient;
        }

        public override async Task<List<TeamListModel>> GetAllAsync()
        {
            var teamList = new List<TeamListModel>();

            var teams = await _apiClient.TeamGetAsync();
            teamList.AddRange(teams);

            return teamList;
        }

        public override async Task<TeamDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.TeamGetAsync(id);
        }

        public override async Task<Guid> CreateAsync(TeamDetailModel data)
        {
            return await _apiClient.TeamPostAsync(data);
        }

        public override async Task<Guid> UpdateAsync(TeamDetailModel data)
        {
            return await _apiClient.TeamPutAsync(data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.TeamDeleteAsync(id);
        }

        public override async Task<List<TeamListModel>> SearchAsync(string search)
        {
            var teamList = new List<TeamListModel>();

            var teams = await _apiClient.SearchAsync(search);
            teamList.AddRange(teams);

            return teamList;
        }
    }
}
