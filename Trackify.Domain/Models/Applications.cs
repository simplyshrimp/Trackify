using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models.Enums;

namespace Trackify.Domain.Models
{
    public class Applications
    {
        public int Id { get; set; }
        public Users User { get; set; }
        public DateOnly ApplicationDate { get; set; }
        public AStatus Status { get; set; }

        public Applications(int id, Users user, DateOnly applicationDate, AStatus status)
        {
            this.Id = id;
            this.User = user;
            this.ApplicationDate = applicationDate;
            this.Status = status;
        }
    }
}
