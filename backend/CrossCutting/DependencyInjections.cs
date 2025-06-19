using Domain.Interfaces.Services;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Interfaces;
using Infrastructure.Utils;
using Domain.Profiles;

namespace CrossCutting
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            #region Business Entities
            services.AddTransient<IUserService, UserService>();            
            services.AddTransient<IAuthService, AuthService>();
            #endregion Business Entities

            #region Automapper
            services.AddAutoMapper(typeof(UserProfile));            
            #endregion Automapper

            #region Utilities            
            services.AddTransient<IPaginationService, PaginationService>();
            #endregion Utilities           

            return services;
        }
    }
}
