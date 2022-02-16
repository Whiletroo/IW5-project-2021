using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web.Pages
{
    public partial class MatchListPage
    {
        [Inject]
        private MatchFacade MatchFacade { get; set; } = null!;

        private ICollection<MatchListModel> MatchList { get; set; } = new List<MatchListModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            MatchList = await MatchFacade.GetAllAsync();
        }
    }
}