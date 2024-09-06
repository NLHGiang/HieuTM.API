using HieuTM.API.B1.MapperProfiles;
using HieuTM.API.B1.Services;

namespace HieuTM.API.B1.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();

        return services;
    }

    public static IServiceCollection AddFluentValidator(this IServiceCollection services)
    {
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();
        services.AddScoped<ISinhVienService, SinhVienService>();

        return services;
    }

    public static IServiceCollection AddMapperProfile(this IServiceCollection services)
    {
        services.AddAutoMapper(c => c.AddProfile(new SinhVienProfile()));

        return services;
    }
}
