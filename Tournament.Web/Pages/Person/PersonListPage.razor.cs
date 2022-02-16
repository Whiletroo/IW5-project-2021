using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web.Pages
{
    public partial class PersonListPage
    {
        [Inject]
        private PersonFacade PersonFacade { get; set; } = null!;

        private ICollection<PersonListModel> PersonList { get; set; } = new List<PersonListModel>();

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
            PersonList = await PersonFacade.GetAllAsync();
        }

        private async Task SearchData()
        {
            PersonList = await PersonFacade.SearchAsync(SearchString);
        }
    }
}
