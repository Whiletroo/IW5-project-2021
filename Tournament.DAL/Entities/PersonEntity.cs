using System;

namespace Tournament.DAL.Entities
{
    public class PersonEntity: EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoURL { get; set; }
        public string? Description { get; set; }
        public TeamEntity? Team { get; set; }
        public Guid? TeamId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as PersonEntity;
            return this.FirstName == other.FirstName
                && this.LastName == other.LastName
                && this.PhotoURL == other.PhotoURL
                && this.Description == other.Description
                && this.TeamId.Equals(other.TeamId);
        }

        public override int GetHashCode() => HashCode.Combine(FirstName, LastName, PhotoURL, Description, TeamId);
    }
}
