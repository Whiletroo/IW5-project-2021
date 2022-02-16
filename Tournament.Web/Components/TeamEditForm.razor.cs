using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Netizine.Enums;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web
{
    public partial class TeamEditForm
    {
        [Inject]
        private TeamFacade TeamFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        [Parameter]
        public EventCallback OnModification { get; set; }

        public TeamDetailModel Data { get; set; } = new TeamDetailModel();

        protected override async Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                Data = await TeamFacade.GetByIdAsync(Id);
            }

            await base.OnInitializedAsync();
        }

        public async Task Update()
        {
            await TeamFacade.UpdateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Create()
        {
            await TeamFacade.CreateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Delete()
        {
            await TeamFacade.DeleteAsync(Id);
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