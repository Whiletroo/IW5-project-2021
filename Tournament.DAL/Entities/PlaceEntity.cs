using System;
using System.Collections.Generic;
using System.Linq;

namespace Tournament.DAL.Entities
{
    public class PlaceEntity : EntityBase
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<MatchEntity>? Matches { get; set; } = new List<MatchEntity>();

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as PlaceEntity;
            return this.Name == other.Name
                && this.Description == other.Description
                && Enumerable.SequenceEqual(this.Matches.OrderBy(e => e.Id), other.Matches.OrderBy(e => e.Id));
        }

        public override int GetHashCode() => HashCode.Combine(Name, Description);
    }
}
