using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web
{
    public partial class PlaceEditForm
    {
        [Inject]
        private PlaceFacade PlaceFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        [Parameter]
        public EventCallback OnModification { get; set; }

        public PlaceDetailModel Data { get; set; } = new PlaceDetailModel();

        protected override async Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                Data = await PlaceFacade.GetByIdAsync(Id);
            }

            await base.OnInitializedAsync();
        }

        public async Task Update()
        {
            await PlaceFacade.UpdateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Create()
        {
            await PlaceFacade.CreateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Delete()
        {
            await PlaceFacade.DeleteAsync(Id);
            await NotifyOnModification();
        }

        private async Task NotifyOnModification()
        {
            if (OnModification.HasDelegate)
            {
                await OnModification.InvokeAsync(null);
            }
        }
    }
}
