using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web.Pages
{
    public partial class PlaceListPage
    {
        [Inject]
        private PlaceFacade PlaceFacade { get; set; } = null!;

        private ICollection<PlaceListModel> PlaceList { get; set; } = new List<PlaceListModel>();

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
            PlaceList = await PlaceFacade.GetAllAsync();
        }

        private async Task SearchData()
        {
            PlaceList = await PlaceFacade.SearchAsync(SearchString);
        }
    }
}
