using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web.Pages
{
    public partial class PersonDetailPage
    {
        [Inject]
        private PersonFacade PersonFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        private PersonDetailModel Person { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            Person = await PersonFacade.GetByIdAsync(Id);
        }
    }
}
