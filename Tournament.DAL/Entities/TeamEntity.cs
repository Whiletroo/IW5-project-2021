using System;
using System.Collections.Generic;
using System.Linq;
using Netizine.Enums;

namespace Tournament.DAL.Entities
{
    public class TeamEntity : EntityBase
    {
        public string TeamName { get; set; }
        public string? LogoURL { get; set; }
        public string? Description { get; set; }
        public Country RegistrationCountry { get; set; }
        public ICollection<PersonEntity>? Persons { get; set; } = new List<PersonEntity>();

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as TeamEntity;
            return this.TeamName == other.TeamName
                && this.LogoURL == other.LogoURL
                && this.Description == other.Description
                && this.RegistrationCountry.Equals(other.RegistrationCountry)
                && Enumerable.SequenceEqual(this.Persons.OrderBy(e => e.Id), other.Persons.OrderBy(e => e.Id));
        }

        public override int GetHashCode() => HashCode.Combine(TeamName, LogoURL, Description, RegistrationCountry);
    }
}