using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web.Pages
{
    public partial class TeamDetailPage
    {
        [Inject]
        private TeamFacade TeamFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        private TeamDetailModel Team { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            Team = await TeamFacade.GetByIdAsync(Id);
        }
    }
}

