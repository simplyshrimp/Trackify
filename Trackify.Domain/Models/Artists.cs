using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.Domain.Models
{
    public class Artists
    {
        public int artistID {  get; set; }
        public Users user { get; set; }
        public bool verified { get; set; }

        public Artists(int artistID, Users user, bool verified)
        {
            this.artistID = artistID;
            this.user = user;
            this.verified = verified;
        }
    }
}
