using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;          
        }

        public async Task<bool> AddUser(User user)
        {
            User toPostUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Role = user.Role
            };
            await _userRepository.AddAsync(toPostUser);
            return true;
        }

        public async Task<bool> DeleteUser(User user)
        {
            if (user == null) return false;

            await _userRepository.DeleteAsync(user.Id);
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
           
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {

                await _userRepository.UpdateAsync(user);
            }
            catch (Exception e)
            {
            }

            return await GetUserById(user.Id);
        }

        public async Task<IQueryable<User>> GetAllUsersQuery()
        {
            return await _userRepository.GetAllQueryAsync();
        }
    }
}
