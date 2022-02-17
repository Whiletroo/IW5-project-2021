using System;
using System.ComponentModel.DataAnnotations;

namespace Tournament.Common.Models
{
    public class PlaceListModel : ModelBase
    {
        [Required]
        public string? Name { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as PlaceListModel;
            return this.Name == other.Name;
        }

        public override int GetHashCode() => HashCode.Combine(Name);
    }

}