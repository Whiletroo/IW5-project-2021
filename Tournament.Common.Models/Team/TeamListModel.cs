using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tournament.Common.Models
{
    public class TeamListModel : ModelBase
    {
        public string TeamName { get; init; }
        public string? LogoURL { get; init; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as TeamListModel;
            return this.TeamName == other.TeamName 
                   && this.LogoURL == other.LogoURL;
                   
        }

        public override int GetHashCode() => HashCode.Combine(TeamName, LogoURL);
    }
}
