using Microsoft.AspNetCore.Components;
using System;

namespace Tournament.Web.Pages
{
    public partial class TeamEditPage
    {
        [Inject]
        private NavigationManager navigationManager { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        public void NavigateBack()
        {
            navigationManager.NavigateTo("/teams");
        }
    }
}
