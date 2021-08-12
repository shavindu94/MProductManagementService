using Application.Interfaces;
using Application.Services;
using DataAccess.EFCore.Repositories;
using DataAccess.EFCore.UnitOfWork;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.DependencyMangement
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IProductTypeService, ProductTypeService>();

            services.AddTransient<IProductTypeRepository, ProductTypeRepository>();

            services.AddTransient<IContentService, ContentService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
