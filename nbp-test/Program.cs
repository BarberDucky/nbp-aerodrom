using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_aerodrom_model;
using nbp_autobus_data.DataProvider;

namespace nbp_test
{
    class Program
    {
        static void Main(string[] args)
        {
            // UserDataProvider.RegisterUser("mimi.nish@gmail.com", "mimi", "Ana", "Trifunovic", "15690");
            //User u = UserDataProvider.LogInUser("a", "mimi");
            //UserDataProvider.ChangeInformationUser("039ddd6f-dcea-4887-a6b5-abc80bd159e4", "Nata", "Stamenkovic", "111");

            //AerodromDataProvider.CreateAerodrom("Nis", "Srbija", "INI");
            //AerodromDataProvider.UpdateAerodrom("1d0daf05-8176-474d-ac4c-a772f2ae1f0d", "Beograd", "Srbija", "BG");
            //AerodromDataProvider.RemoveAerodrom("f7cc88da-61cf-404b-ba6b-878cae8582b0");
            //LetDataProvider.CreateLet(new nbp_aerodrom_model.DTOs.CreateLetDTO
            //{
            //    VremeDolaska = DateTime.Now,
            //    VremePolaska = DateTime.Now,
            //    PolazniAeroID = "1d0daf05-8176-474d-ac4c-a772f2ae1f0d",
            //    DolazniAeroID = "1d0daf05-8176-474d-ac4c-a772f2ae1f0d",
            //    PrevoznikID = "1"
            //});

            //      LetDataProvider.Delete("74ad0afe-91a7-4834-b417-cef5c65623b7");

            UserDataProvider.RegisterUser(new nbp_autobus_data.DTOs.UserDTO()
            {
                Id = "2"
            }
            );
           
        }
    }
}
