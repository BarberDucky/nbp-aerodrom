using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.DTOs
{
    public class UserDTO
    {
        public String Id { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String PassportNumber { get; set; }

        public static User FromDTO(UserDTO dto)
        {
            return new User()
            {
                Id = dto.Id,
                Email = dto.Email,
                PassportNumber = dto.PassportNumber,
                Password = dto.Password,
                Name = dto.Name,
                LastName = dto.LastName
            };
        }
        public UserDTO()
        {

        }
        public UserDTO(User u)
        {
            Id = u.Id;
            Email = u.Email;
            PassportNumber = u.PassportNumber;
            Password = u.Password;
            Name = u.Name;
            LastName = u.LastName;
        }
    }

    public class ReadUserDTO
    {
        public String Id { get; set; }
        public String Email { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String PassportNumber { get; set; }

        public ReadUserDTO()
        {

        }
        public ReadUserDTO(User u)
        {
            Id = u.Id;
            Email = u.Email;
            PassportNumber = u.PassportNumber;
            Name = u.Name;
            LastName = u.LastName;
        }
    }
}
