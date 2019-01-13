using nbp_autobus_data.BusinessModel;
using nbp_autobus_data.DataProvider;
using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.DTOs
{
    public class ReadRideDTO
    {

        public string Id { get; set; }
        public int NumberOfSeats { get; set; }
        public float RidePrice { get; set; }
        public DateTime TakeOfTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public CarrierDTO Carrier { get; set; }
        public StationDTO ArrivalStation { get; set; }
        public StationDTO TakeOfStation { get; set; }

        public ReadRideDTO(Ride ride)
        {
            Id = ride.Id;
            NumberOfSeats = ride.NumberOfSeats;
            RidePrice = ride.RidePrice;
            TakeOfTime = ride.TakeOfTime;
            ArrivalTime = ride.ArrivalTime;
            DayOfWeek = ride.DayOfWeek;
        }

        public ReadRideDTO(Ride ride, string carrierId, string arrivalStationId, string takeOfStationId)
            : this(ride)
        {
            Carrier = CarrierDataProvider.GetCarrier(carrierId);
            ArrivalStation = StationDataProvider.Get(arrivalStationId);
            TakeOfStation = StationDataProvider.Get(takeOfStationId);
        }

        public ReadRideDTO()
        {

        }

        public ReadRideDTO(BusinessRide ride)
            : this(ride.Ride)
        {
            Carrier = new CarrierDTO(ride.Carrier);
            ArrivalStation = new StationDTO(ride.ArrivalStation);
            TakeOfStation = new StationDTO(ride.TakeOfStation);
        }

        public static IEnumerable<ReadRideDTO> FromEntityList(IEnumerable<BusinessRide> c)
        {
            List<ReadRideDTO> list = new List<ReadRideDTO>();
            if (c != null && c.Count() > 0)
            {
                foreach (var el in c)
                {
                    var dto = new ReadRideDTO(el);
                    list.Add(dto);
                }
            }

            return list;
        }
    }

    public class CreateRideDTO
    {
        public int NumberOfSeats { get; set; }
        public float RidePrice { get; set; }
        public DateTime TakeOfTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public string CarrierId { get; set; }
        public string TakeOfStationId { get; set; }
        public string ArrivalStationId { get; set; }

        public static Ride FromDTO(CreateRideDTO dto)
        {
            return new Ride()
            {
                NumberOfSeats = dto.NumberOfSeats,
                RidePrice = dto.RidePrice,
                TakeOfTime = dto.TakeOfTime,
                ArrivalTime = dto.ArrivalTime,
                DayOfWeek = dto.DayOfWeek
            };
        }
    }

    public class UpdateRideDTO
    {
        public int NumberOfSeats { get; set; }
        public float RidePrice { get; set; }
        public DateTime TakeOfTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public static Ride FromDTO(UpdateRideDTO dto)
        {
            return new Ride()
            {
                NumberOfSeats = dto.NumberOfSeats,
                RidePrice = dto.RidePrice,
                TakeOfTime = dto.TakeOfTime,
                ArrivalTime = dto.ArrivalTime,
                DayOfWeek = dto.DayOfWeek
            };
        }
    }

    public class RideOfCarrierDTO
    {
        public string Id { get; set; }
        public int NumberOfSeats { get; set; }
        public float RidePrice { get; set; }
        public DateTime TakeOfTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public StationDTO ArrivalStation { get; set; }
        public StationDTO TakeOfStation { get; set; }

        public RideOfCarrierDTO(Ride ride)
        {
            Id = ride.Id;
            NumberOfSeats = ride.NumberOfSeats;
            RidePrice = ride.RidePrice;
            TakeOfTime = ride.TakeOfTime;
            ArrivalTime = ride.ArrivalTime;
            DayOfWeek = ride.DayOfWeek;
        }
        public RideOfCarrierDTO(BusinessRide ride)
            : this(ride.Ride)
        {
            ArrivalStation = new StationDTO(ride.ArrivalStation);
            TakeOfStation = new StationDTO(ride.TakeOfStation);
        }
    }
}
