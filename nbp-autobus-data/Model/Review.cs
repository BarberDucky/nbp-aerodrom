using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.Model
{
    public class Review
    {
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public String Text { get; set; }
        public int Grade { get; set; }
        //Iz redisa
       // public float AvgGrade { get; set; }

        public Carrier Carrier { get; set; }
        public User User { get; set; }

        public Review()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
