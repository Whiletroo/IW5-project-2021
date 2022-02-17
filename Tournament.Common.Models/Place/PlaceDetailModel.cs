using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Tournament.Common.Models
{
    public class PlaceDetailModel : ModelBase
    {
        [Required(ErrorMessage = "Place name is required.")]
        public string Name {  get; set; }
        public string? Description {  get; set; }
        public ICollection<MatchListModel>? Matches {get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as PlaceDetailModel;
            return this.Name == other.Name
                   && this.Description == other.Description
                   && Enumerable.SequenceEqual(this.Matches.OrderBy(e => e.Id), other.Matches.OrderBy(e => e.Id));
        }

        public override int GetHashCode() => HashCode.Combine(Name, Description);
    }
}