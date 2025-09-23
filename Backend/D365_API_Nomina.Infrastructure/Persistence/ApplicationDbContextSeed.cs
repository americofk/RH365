using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            //Insertar data en la tabla de menu
            List<MenuApp> menuApps = new List<MenuApp>()
            {
                new MenuApp(){ MenuId= MenuConst.ReportConfig, MenuName="Configuración", Description="Título de configuración", Icon="fa fa-setting", Action="Click"},
                new MenuApp(){ MenuId="MENU-0002", MenuName="Colaboradores", Description="Listado de colaboradores", Icon="fa fa-user", Action="Click", MenuFather=MenuConst.ReportConfig}
            };

            modelBuilder.Entity<MenuApp>().HasData(menuApps);

            //Insertar la empresa raiz
            Company company = new Company()
            {
                CompanyId = "Root",
                Name = "Empresa raiz",
                Email = "",
                Responsible = "Administrator"
            };

            modelBuilder.Entity<Company>().HasData(company);

            //Insertar códigos de formato
            List<FormatCode> formatCodes = new List<FormatCode>()
            {
                new FormatCode()  { FormatCodeId = "en-US", Name = "Estado Únidos"},
                new FormatCode()  { FormatCodeId = "es-ES", Name = "España"},
            };

            modelBuilder.Entity<FormatCode>().HasData(formatCodes);

            //Insertar usuario administrador global
            User user = new User()
            {
                Alias = "Admin",
                Name = "Admin",
                Email = "fflores@dynacorp365.com",
                Password = SecurityHelper.MD5("123456"),
                ElevationType = Core.Domain.Enums.AdminType.LocalAdmin,
                CompanyDefaultId = "Root",
                FormatCodeId = "en-US",
            };

            modelBuilder.Entity<User>().HasData(user);

            //Insertar monedas
            List<Currency> currencies = new List<Currency>()
            {
                new Currency()  { CurrencyId = "USD", Name = "Dólares"},
                new Currency()  { CurrencyId = "DOP", Name = "Pesos Dominicanos"},
            };

            modelBuilder.Entity<Currency>().HasData(currencies);

            //Insertar paises
            List<Country> countries = new List<Country>()
            {
                new Country()  {CountryId = "DOM", Name = "República Dominicana"},
                new Country()  {CountryId = "CH", Name = "Chile"},
            };

            modelBuilder.Entity<Country>().HasData(countries);
        }
    }
}
