using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tournament.Common.Models;
using Tournament.Web.BL.Facades;

namespace Tournament.Web
{
    public partial class MatchEditForm
    {
        [Inject]
        private MatchFacade MatchFacade { get; set; } = null!;
        [Inject]
        private TeamFacade TeamFacade { get; set; } = null!;
        [Inject] 
        private PlaceFacade PlaceFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        [Parameter]
        public EventCallback OnModification { get; set; }

        public MatchDetailModel Data { get; set; } = new MatchDetailModel();

        public ICollection<TeamListModel> TeamList { get; set; } = new List<TeamListModel>();
        public ICollection<PlaceListModel> PlaceList { get; set; } = new List<PlaceListModel>();
        private Guid? SelectedTeam1Id { get; set; } = Guid.Empty;
        private Guid? SelectedTeam2Id { get; set; } = Guid.Empty;
        private Guid? SelectedPlaceId { get; set; } = Guid.Empty;
        private DateTime? SelectedDateTime { get; set; } = DateTime.Now;

        protected override async Task OnInitializedAsync()
        {
            TeamList = await TeamFacade.GetAllAsync();
            PlaceList = await PlaceFacade.GetAllAsync();
            if (Id != Guid.Empty)
            {
                Data = await MatchFacade.GetByIdAsync(Id);
                SelectedTeam1Id = Data.Team1Id;
                SelectedTeam2Id = Data.Team2Id;
                SelectedPlaceId = Data.PlaceId;
                SelectedDateTime = Data.DateTime;
            }
            await base.OnInitializedAsync();
        }

        public async Task Update()
        {
            Data.Team1Id = SelectedTeam1Id != Guid.Empty ? SelectedTeam1Id : null;
            Data.Team2Id = SelectedTeam2Id != Guid.Empty ? SelectedTeam2Id : null;
            Data.PlaceId = SelectedPlaceId;
            Data.DateTime = SelectedDateTime;
            await MatchFacade.UpdateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Create()
        {
            Data.Team1Id = SelectedTeam1Id != Guid.Empty ? SelectedTeam1Id : null;
            Data.Team2Id = SelectedTeam2Id != Guid.Empty ? SelectedTeam2Id : null;
            Data.PlaceId = SelectedPlaceId;
            Data.DateTime = SelectedDateTime;
            await MatchFacade.CreateAsync(Data);
            Data = new();
            await NotifyOnModification();

        }

        public async Task Delete()
        {
            await MatchFacade.DeleteAsync(Id);
            await NotifyOnModification();
        }

        private async Task NotifyOnModification()
        {
            if (OnModification.HasDelegate)
            {
                await OnModification.InvokeAsync(null);
            }
        }

        public Guid SelectedATeam
        {
            get { return SelectedTeam1Id ?? Guid.Empty; }
            set
            {
                SelectedTeam1Id = value;
                if (!SelectedATeam.Equals(Guid.Empty) && SelectedATeam.Equals(SelectedBTeam)) SelectedBTeam = Guid.Empty;
            }
        }

        public Guid SelectedBTeam
        {
            get { return SelectedTeam2Id ?? Guid.Empty; }
            set
            {
                SelectedTeam2Id = value;
                if (!SelectedBTeam.Equals(Guid.Empty) && SelectedBTeam.Equals(SelectedATeam)) SelectedATeam = Guid.Empty;
            }
        }

        public Guid SelectedPlace
        {
            get { return SelectedPlaceId ?? Guid.Empty; }
            set { SelectedPlaceId = value; }
        }

        public DateOnly SelectedDate
        {
            get
            {
                if (SelectedDateTime != null)
                {
                    return DateOnly.Parse(SelectedDateTime.Value.ToString("MM/dd/yyyy"));
                }

                return DateOnly.Parse(DateTime.Now.ToString("MM/dd/yyyy"));
            }
            set
            {
                SelectedDateTime = DateTime.Parse(value.ToString("MM/dd/yyyy") + " " + SelectedTime);
            }
        }

        public TimeOnly SelectedTime
        {
            get
            {
                if (SelectedDateTime != null)
                {
                    return TimeOnly.Parse(SelectedDateTime.Value.ToString("HH:mm"));
                }

                return TimeOnly.Parse(DateTime.Now.ToString("HH:mm"));
            }
            set
            {
                SelectedDateTime = DateTime.Parse(SelectedDate + " " + value.ToString("HH:mm"));
            }
        }

        //public DateTime dateTime
        //{
        //    get { return SelectedDateTime ?? DateTime.Now; }
        //    set
        //    {
        //        SelectedDateTime = value;
        //    }
        //}
    }
}
