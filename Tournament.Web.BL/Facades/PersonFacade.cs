using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Common.Models;

namespace Tournament.Web.BL.Facades
{
    public class PersonFacade : FacadeBase<PersonDetailModel, PersonListModel>
    {
        private readonly IPersonClient _apiClient;

        public PersonFacade(IPersonClient apiClient)
        {
            _apiClient = apiClient;
        }

        public override async Task<List<PersonListModel>> GetAllAsync()
        {
            var personList = new List<PersonListModel>();

            var persons = await _apiClient.PersonGetAsync();
            personList.AddRange(persons);

            return personList;
        }

        public override async Task<PersonDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.PersonGetAsync(id);
        }

        public override async Task<Guid> CreateAsync(PersonDetailModel data)
        {
            return await _apiClient.PersonPostAsync(data);
        }

        public override async Task<Guid> UpdateAsync(PersonDetailModel data)
        {
            return await _apiClient.PersonPutAsync(data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.PersonDeleteAsync(id);
        }

        public override async Task<List<PersonListModel>> SearchAsync(string search)
        {
            var personList = new List<PersonListModel>();

            var persons = await _apiClient.SearchAsync(search);
            personList.AddRange(persons);

            return personList;
        }
    }
}
