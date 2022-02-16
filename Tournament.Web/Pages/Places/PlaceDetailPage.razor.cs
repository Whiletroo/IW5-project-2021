using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web.Pages
{
    public partial class PlaceDetailPage
    {
        [Inject]
        private PlaceFacade PlaceFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        private PlaceDetailModel Place { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            Place = await PlaceFacade.GetByIdAsync(Id);
        }
    }
}
