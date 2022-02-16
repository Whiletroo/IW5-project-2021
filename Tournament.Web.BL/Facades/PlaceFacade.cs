using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Common.Models;

namespace Tournament.Web.BL.Facades
{
    public class PlaceFacade : FacadeBase<PlaceDetailModel, PlaceListModel>
    {
        private readonly IPlaceClient _apiClient;

        public PlaceFacade(IPlaceClient apiClient)
        {
            _apiClient = apiClient;
        }

        public override async Task<List<PlaceListModel>> GetAllAsync()
        {
            var placeList = new List<PlaceListModel>();

            var places = await _apiClient.PlaceGetAsync();
            placeList.AddRange(places);

            return placeList;
        }

        public override async Task<PlaceDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.PlaceGetAsync(id);
        }

        public override async Task<Guid> CreateAsync(PlaceDetailModel data)
        {
            return await _apiClient.PlacePostAsync(data);
        }

        public override async Task<Guid> UpdateAsync(PlaceDetailModel data)
        {
            return await _apiClient.PlacePutAsync(data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.PlaceDeleteAsync(id);
        }

        public override async Task<List<PlaceListModel>> SearchAsync(string search)
        {
            var placeList = new List<PlaceListModel>();

            var places = await _apiClient.SearchAsync(search);
            placeList.AddRange(places);

            return placeList;
        }
    }
}
