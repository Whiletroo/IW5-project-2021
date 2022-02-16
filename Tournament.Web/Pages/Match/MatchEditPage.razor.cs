using Microsoft.AspNetCore.Components;
using System;

namespace Tournament.Web.Pages
{
    public partial class MatchEditPage
    {
        [Inject]
        private NavigationManager navigationManager { get; set; } = null!;

        [Parameter]
        public Guid Id { get; set; }

        public void NavigateBack()
        {
            navigationManager.NavigateTo("/matches");
        }
    }
}
