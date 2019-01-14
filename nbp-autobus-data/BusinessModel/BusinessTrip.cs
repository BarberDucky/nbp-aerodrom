using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.BusinessModel
{
    public class BusinessTrip
    {
        public List<BusinessCard> CardsInTrip { get; set; }
        public float TotalCost { get; set; }
        public int OverlayNumber { get; set; }

        public BusinessTrip()
        {
            TotalCost = 0;
            OverlayNumber = 0;
            CardsInTrip = new List<BusinessCard>();
        }
    }
}
