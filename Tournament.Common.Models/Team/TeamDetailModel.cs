using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Netizine.Enums;
using Tournament.DAL.Enums;

namespace Tournament.Common.Models
{
    public class TeamDetailModel : ModelBase
    {
        [Required(ErrorMessage = "Team name is required.")]
        public string TeamName { get; set; }
        public string? LogoURL { get; set; }
        public string? Description { get; set; }
        public Country RegistrationCountry { get; set; }
        public ICollection<PersonListModel>? Persons { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as TeamDetailModel;
            return this.TeamName == other.TeamName
                   && this.LogoURL == other.LogoURL
                   && this.Description == other.Description
                   && this.RegistrationCountry == other.RegistrationCountry
                   && Enumerable.SequenceEqual(this.Persons.OrderBy(e => e.Id), other.Persons.OrderBy(e => e.Id));
        }

        public override int GetHashCode() => HashCode.Combine(TeamName, LogoURL, Description, RegistrationCountry, Persons);
    }
}
