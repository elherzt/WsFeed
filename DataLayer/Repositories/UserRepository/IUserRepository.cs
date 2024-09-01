using DataLayer.Models;
using DataLayer.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<Response> GetUserByIdAsync(int userId);
        Task<Response> GetUserByEmailAsync(string email);
        Task<Response> AddUserAsync(User user);
        Task<Response> LoginAsync(User user);
        Task<Response> SaveLoginDateAsync(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response> GetUserByIdAsync(int userId)
        {
            Response response = new Response(TypeOfResponse.OK, "User found");

            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    response.TypeOfResponse = TypeOfResponse.NotFound;
                    response.Message = "User not found";
                }
                else
                {
                    response.Data = user;
                }
            }
            catch(Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }   


            
            return response;
        }

        public async Task<Response> GetUserByEmailAsync(string email)
        {
            Response response = new Response(TypeOfResponse.OK, "User found");

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Mail == email);
                if (user == null)
                {
                    response.TypeOfResponse = TypeOfResponse.NotFound;
                    response.Message = "User not found";
                }
                else
                {
                    response.Data = user;
                }
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }

            return response;
            
        }

        public async Task<Response> LoginAsync(User user)
        {
            Response response = new Response(TypeOfResponse.OK, "User found");

            try
            {
                var existingUser = await GetUserByEmailAsync(user.Mail);
                if (existingUser.TypeOfResponse != TypeOfResponse.OK)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "User not found";
                    return response;
                }

                User userDB = (User)existingUser.Data;
                if (!Cryptography.Verify(user.Password, userDB.Password))
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "Invalid password";
                    return response;
                }

                var lastLogin = DateTime.Now;
                var saveLogin = await SaveLoginDateAsync(user);

                if (saveLogin.TypeOfResponse == TypeOfResponse.OK)
                {
                    lastLogin = (DateTime)saveLogin.Data;
                }



                UserDTOResponse userDTOResponse = new UserDTOResponse();
                userDTOResponse.Id = userDB.Id;
                userDTOResponse.Name = userDB.Name;
                userDTOResponse.Mail = userDB.Mail;
                userDTOResponse.LastLogin = lastLogin;
                response.Data = userDTOResponse;
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<Response> AddUserAsync(User user)
        {
            
            Response response = new Response(TypeOfResponse.OK, "User created successfully");

            try
            {
                var existingUser = await GetUserByEmailAsync(user.Mail);
                if (existingUser.TypeOfResponse == TypeOfResponse.OK)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "User already exists";
                    return response;
                }

                user.Password = Cryptography.Encrypt(user.Password);
                user.CreatedDate = DateTime.Now;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                UserDTOResponse userDTOResponse = new UserDTOResponse();
                userDTOResponse.Id = user.Id;
                userDTOResponse.Name = user.Name;
                userDTOResponse.Mail = user.Mail;
                response.Data = userDTOResponse;
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }

            return response;


        }

        public async Task<Response> SaveLoginDateAsync(User user)
        {
            Response response = new Response(TypeOfResponse.OK, "Login date saved successfully");
            try
            {
                var existingUser = await GetUserByEmailAsync(user.Mail);
                if (existingUser.TypeOfResponse != TypeOfResponse.OK)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "User not found";
                    return response;
                }

                User userDB = (User)existingUser.Data;
                DateTime lastLogin = userDB.LastLoginDate.HasValue ? userDB.LastLoginDate.Value : DateTime.Now;
                response.Data = lastLogin;
                userDB.LastLoginDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
