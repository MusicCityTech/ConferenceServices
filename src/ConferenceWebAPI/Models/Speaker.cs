using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceWebAPI.Models
{
    public class Speaker
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}