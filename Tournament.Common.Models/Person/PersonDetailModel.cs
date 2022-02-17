using System;
using System.ComponentModel.DataAnnotations;

namespace Tournament.Common.Models
{
    public class PersonDetailModel : ModelBase
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        public string? PhotoURL { get; set; }
        public string? Description { get; set; }

        public Guid? TeamId { get; set; }
        public string? TeamName { get; set; }
        public string? TeamLogoURL { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as PersonDetailModel;
            return this.FirstName == other.FirstName
                && this.LastName == other.LastName
                && this.PhotoURL == other.PhotoURL
                && this.Description == other.Description
                && this.TeamId.Equals(other.TeamId)
                && this.TeamName == other.TeamName
                && this.TeamLogoURL == other.TeamLogoURL;
        }

        public override int GetHashCode() => HashCode.Combine(FirstName, LastName, PhotoURL, Description, TeamId, TeamName, TeamLogoURL);
    }
}
