using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.Domain.Models
{
    public class Genres
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public bool Deleted { get; set; }
    }
}
