using System;
using System.ComponentModel.DataAnnotations;
using Tournament.DAL.Enums;

namespace Tournament.Common.Models
{
    public class MatchDetailModel : ModelBase
    {
        public Guid? Team1Id { get; set; }
        public string? Team1Name { get; set; }
        public string? Team1LogoURL { get; set; }
        public Guid? Team2Id { get; set; }
        public string? Team2Name { get; set; }
        public string? Team2LogoURL { get; set; }
        public string PlaceName { get; set; }
        [Required(ErrorMessage = "Match place is required.")]
        public Guid? PlaceId { get; set; }
        [Required(ErrorMessage = "Match date and time is required.")]
        public DateTime? DateTime { get; set; }
        [Required(ErrorMessage = "Match result is required.")]
        public Results? Result { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as MatchDetailModel;
            return this.Team1Id.Equals(other.Team1Id)
                && this.Team1Name == other.Team1Name
                && this.Team1LogoURL == other.Team1LogoURL
                && this.Team2Id.Equals(other.Team2Id)
                && this.Team2Name == other.Team2Name
                && this.Team2LogoURL == other.Team2LogoURL
                && this.PlaceId.Equals(other.PlaceId)
                && this.PlaceName == other.PlaceName
                && this.DateTime.Equals(other.DateTime)
                && this.Result == other.Result;
        }

        public override int GetHashCode() => HashCode.Combine(Team1Id, Team1Name, Team2Id, Team2Name, PlaceId, PlaceName, DateTime, Result);
    } 
}