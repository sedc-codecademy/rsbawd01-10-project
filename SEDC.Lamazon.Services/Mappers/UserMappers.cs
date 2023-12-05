using SEDC.Lamazon.Domain.Entities;
using SEDC.Lamazon.Services.ViewModels.Role;
using SEDC.Lamazon.Services.ViewModels.User;

namespace SEDC.Lamazon.Services.Mappers;

public static class UserMappers
{
    public static UserViewModel ToUserViewModel(this User model)
    {
        return new UserViewModel()
        {
            City = model.City,
            Email = model.Email,
            FullName = model.FullName,
            Id = model.Id,
            Password = model.Password,
            PhoneNumber = model.PhoneNumber,
            Role = new RoleViewModel()
            {
                Id = model.Role.Id,
                Key = model.Role.Key,
                Name = model.Role.Name
            },
            PostalCode = model.PostalCode,
            State = model.State,
            StreetAdress = model.StreetAdress,
        };
    }
}
