using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models;

namespace Trackify.Service
{
    public class Other
    {
        public TimeSpan GetTotalDuration(List<Songs> songList)
        {
            TimeSpan totalDuration = TimeSpan.Zero;
            foreach (Songs song in songList)
            {
                totalDuration.Add(song.length);
            }
            return totalDuration;
        }
    }
}
