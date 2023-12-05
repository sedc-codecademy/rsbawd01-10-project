using Microsoft.AspNetCore.Identity;
using SEDC.Lamazon.DataAccess.Implementations;
using SEDC.Lamazon.DataAccess.Interfaces;
using SEDC.Lamazon.Domain.Entities;
using SEDC.Lamazon.Services.Interfaces;
using SEDC.Lamazon.Services.Mappers;
using SEDC.Lamazon.Services.ViewModels.Role;
using SEDC.Lamazon.Services.ViewModels.User;

namespace SEDC.Lamazon.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public UserViewModel LoginUser(LoginUserViewModel loginUserViewModel)
    {
        User user = _userRepository.GetUserByEmail(loginUserViewModel.Email);

        if (user is null)
            throw new Exception("Login credentials do not match any user");

        PasswordVerificationResult passwordVerificationResult = 
            _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserViewModel.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new Exception("Login credentials do not match any user");

        return user.ToUserViewModel();
    }

    public void RegisterUser(RegisterUserViewModel registerUserViewModel)
    {
        if (registerUserViewModel == null)
            throw new Exception("Model is null");

        if (registerUserViewModel.Password != registerUserViewModel.ConfirmationPassword)
            throw new Exception("Passwords must match");

        // create new user
        User newUser = new User()
        {
            City = registerUserViewModel.City,
            Email = registerUserViewModel.Email,
            FullName = registerUserViewModel.FullName,
            PhoneNumber = registerUserViewModel.PhoneNumber,
            PostalCode = registerUserViewModel.PostalCode,
            State = registerUserViewModel.State,
            StreetAdress = registerUserViewModel.StreetAdress,
        };

        // Hash the password
        string hashPassword = _passwordHasher.HashPassword(newUser, registerUserViewModel.Password);
        newUser.Password = hashPassword;

        // Get Role data
        Role role = _userRepository.GetUserRole(registerUserViewModel.RoleId);

        if (role == null)
            throw new Exception("The role does not exists");

        newUser.Role = role;

        // save to db
        _userRepository.Insert(newUser);
    }

    public List<UserViewModel> GetAllUsers()
    {
        return _userRepository
            .GetAll()
            .Select(ur => ur.ToUserViewModel())
            .ToList();
    }

    public UserViewModel GetUserById(int id)
    {
        return _userRepository
            .Get(id)
            .ToUserViewModel();
    }

    public void DeleteUserById(int id)
    {
        _userRepository.Delete(id);
    }

    public RoleViewModel GetUserRoleById(int id)
    {
        Role dbRole = _userRepository.GetUserRole(id);

        return new RoleViewModel()
        {
            Id = dbRole.Id,
            Key = dbRole.Key,
            Name = dbRole.Name,
        };
    }

    public List<RoleViewModel> GetAllUserRoles()
    {
        return _userRepository.GetAllUserRoles()
            .Select(ur => new RoleViewModel()
            {
                Id = ur.Id,
                Name = ur.Name,
                Key = ur.Key
            }).ToList();
    }
}
