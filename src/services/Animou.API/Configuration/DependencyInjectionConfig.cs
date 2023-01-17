using Animou.Business.Interfaces;
using Animou.Business.Models;
using Animou.Business.Notifications;
using Animou.Business.Services;
using Animou.Data.Context;
using Animou.Data.Repository;

namespace Animou.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<AnimouContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IDislikeRepository, DislikeRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRelationsService, RelationsService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<INotifier, Notifier>();
        }
    }
}
