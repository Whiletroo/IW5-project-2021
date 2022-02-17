using System;
using Tournament.DAL.Enums;

namespace Tournament.DAL.Entities
{
    public class MatchEntity : EntityBase
    {
        public TeamEntity? Team1 { get; set; }
        public Guid? Team1Id { get; set; }
        public TeamEntity? Team2 { get; set; }
        public Guid? Team2Id { get; set; }
        public PlaceEntity Place { get; set; }
        public Guid PlaceId { get; set; }
        public DateTime DateTime { get; set; }
        public Results Result { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as MatchEntity;
            return this.Team1Id.Equals(other.Team1Id)
                && this.Team2Id.Equals(other.Team2Id)
                && this.PlaceId.Equals(other.PlaceId)
                && this.DateTime.Equals(other.DateTime)
                && this.Result.Equals(other.Result);
        }

        public override int GetHashCode() => HashCode.Combine(Team1Id, Team2Id, PlaceId, DateTime, Result);
    }
}
