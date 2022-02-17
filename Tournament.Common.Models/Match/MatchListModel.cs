using System;
using Tournament.Common.Models.Enums;

namespace Tournament.Common.Models
{
    public class MatchListModel : ModelBase
    {
        public Guid? Team1Id { get; set; }
        public string? Team1Name { get; set; }
        public Guid? Team2Id { get; set; }
        public string? Team2Name { get; set; }
        public DateTime? DateTime { get; set; }
        public Results Result { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as MatchListModel;
            return this.Team1Id.Equals(other.Team1Id)
                   && this.Team2Id.Equals(other.Team2Id)
                   && this.Team1Name == other.Team1Name
                   && this.Team2Name == other.Team2Name
                   && this.DateTime == other.DateTime
                   && this.Result == other.Result;
        }

        public override int GetHashCode() => HashCode.Combine(Team1Id, Team2Id, Team1Name, Team2Name, DateTime, Result);
    }
}