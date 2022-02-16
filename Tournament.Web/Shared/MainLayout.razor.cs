using Microsoft.AspNetCore.Components;
using Tournament.Web.BL.Facades;

namespace Tournament.Web.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public PersonFacade PersonFacade { get; set; } = null!;
    }
}
