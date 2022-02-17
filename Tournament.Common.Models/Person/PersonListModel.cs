using System;

namespace Tournament.Common.Models
{
    public class PersonListModel : ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoURL { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as PersonListModel;
            return this.FirstName == other.FirstName
                && this.LastName == other.LastName
                && this.PhotoURL == other.PhotoURL;
        }

        public override int GetHashCode() => HashCode.Combine(FirstName, LastName, PhotoURL);
    }
}
