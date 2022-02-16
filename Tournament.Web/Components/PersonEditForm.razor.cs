using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web
{
    public partial class PersonEditForm
    {
        [Inject]
        private PersonFacade PersonFacade { get; set; } = null!;
        [Inject]
        private TeamFacade TeamFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        [Parameter]
        public EventCallback OnModification { get; set; }

        public PersonDetailModel Data { get; set; } = new PersonDetailModel();

        public ICollection<TeamListModel> TeamList { get; set; } = new List<TeamListModel>();
        private Guid? SelectedTeamId { get; set; } = Guid.Empty;


        protected override async Task OnInitializedAsync()
        {
            TeamList = await TeamFacade.GetAllAsync();

            if (Id != Guid.Empty)
            {
                Data = await PersonFacade.GetByIdAsync(Id);
                SelectedTeamId = Data.TeamId ?? Guid.Empty;
            }

            await base.OnInitializedAsync();
        }

        public async Task Update()
        {
            Data.TeamId = SelectedTeamId != Guid.Empty ? SelectedTeamId : null;
            await PersonFacade.UpdateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Create()
        {
            Data.TeamId = SelectedTeamId != Guid.Empty ? SelectedTeamId : null;
            await PersonFacade.CreateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Delete()
        {
            await PersonFacade.DeleteAsync(Id);
            await NotifyOnModification();
        }

        private async Task NotifyOnModification()
        {
            if (OnModification.HasDelegate)
            {
                await OnModification.InvokeAsync(null);
            }
        }

        private Guid SelectedTeam
        {
            get { return SelectedTeamId ?? Guid.Empty; }
            set { SelectedTeamId = value; }
        }
    }
}
