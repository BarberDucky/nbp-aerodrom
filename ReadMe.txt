						
						API
------------------------------------------------------------------------
USER CONTROLLER

UserDTO
{

"Id" : "nestonesto"
"PassportNumber": "Uros",

"Email": "vukic@gmail.com",

"Password" :  "Pass123!",

"Name": "Milca",

"LastName": "Vukic"

}

 - Register user
POST: api/User
prima: UserDTO
povratni tip: userId - string

- Login user
POST: api/User/Login
prima: UserDTO, treba mu samo email i password
povratni tip: UserDTO

- Update user
PUT: api/User/id
prima: UserDTO
povratni tip: bool
--------------------------------------------------------------------------
CARRIER CONTROLER

CarrierDTO
{
	
"Name": "Lasta",
	
"Website": "www.lasta.com",
	
"PhoneNumber" : "064 99 11 862",
	
"UserId" : "10d4d892-d6ce-4039-b1c9-c76c859e7663"

}

 - Insert carrier
POST: api/Carrier
prima: CarrierDTO
povratni tip: carrierId - string ili null ako ne uspe

 - Update carrier
PUT: api/Carrier/carrierId
prima: CarrierDTO
povratni tip: bool

 - Get carrier
GET: api/Carrier/carrierId
povratni tip: CarrierDTO

 - Get carriers by user
GET: api/Carrier/GetCarriersByUser/{userId}
povratni tip: List<CarrierDTO>
--------------------------------------------------------------------------------
STATION CONTROLLER

StationDTO
{
	"Name": "BUS Beograd",
	"City": "Beograd",
	"Country": "Serbia"
}

 - Insert station
POST: api/Station
prima: StationDTO
povratni tip: StationDTO ili null ako ne uspe

 - Get station
GET: api/Station/station
povratni tip: StationDTO

 - Get all stations
GET: api/Station
povratni tip: List<StationDTO>