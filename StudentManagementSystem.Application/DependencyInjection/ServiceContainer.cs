using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Application.ValidationService;

namespace StudentManagementSystem.Application.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        // Register MediatR with validation pipeline behavior
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceContainer).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
        });

        //Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(ServiceContainer).Assembly);
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        //// Optional: Configure validation options
        services.Configure<FluentValidationAutoValidationConfiguration>(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });

        return services;
    }
}
