using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web.Pages
{
    public partial class TeamListPage
    {
        [Inject]
        private TeamFacade TeamFacade { get; set; } = null!;

        private ICollection<TeamListModel> TeamList { get; set; } = new List<TeamListModel>();

        private string SearchString { get; set; } = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }

        private async Task OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                await LoadData();
            }
            else
            {
                await SearchData();
            }

            StateHasChanged();
        }

        private async Task LoadData()
        {
            TeamList = await TeamFacade.GetAllAsync();
        }

        private async Task SearchData()
        {
            TeamList = await TeamFacade.SearchAsync(SearchString);
        }
    }
}