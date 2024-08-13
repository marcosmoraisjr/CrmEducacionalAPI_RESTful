using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMEducacional.Data; // Importa o contexto do banco de dados
using Microsoft.AspNetCore.Builder; // Importa funcionalidades para configurar o pipeline HTTP
using Microsoft.AspNetCore.Hosting; // Importa funcionalidades para hospedar a aplicação
using Microsoft.AspNetCore.Http; // Importa funcionalidades para manipulação de requisições HTTP
using Microsoft.Extensions.DependencyInjection; // Importa funcionalidades para injeção de dependência
using Microsoft.Extensions.Hosting; // Importa funcionalidades para configuração do host
using Microsoft.OpenApi.Models; // Importa modelos para configuração do Swagger

namespace CrmEducacional
{
    /// <summary>
    /// Classe de configuração da aplicação. Contém métodos para configurar serviços e o pipeline de requisições HTTP.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Este método é chamado pelo runtime. Use este método para adicionar serviços ao contêiner.
        /// </summary>
        /// <param name="services">O contêiner de serviços onde os serviços são adicionados.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Adiciona suporte para controllers e views
            services.AddControllersWithViews();
            // Adiciona suporte para controllers API
            services.AddControllers();
            // Adiciona o contexto do banco de dados ao contêiner de serviços
            services.AddDbContext<AppDbContext>();

            // Configuração do Swagger para geração de documentação da API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1", // Versão da API
                    Title = "API CrmEducacional", // Título da API
                    Description = "Documentação da API do CrmEducacional", // Descrição da API
                    Contact = new OpenApiContact
                    {
                        Name = "Marcos Morais", // Nome do responsável
                        Email = "mmstec@example.com" // Email do responsável
                    }
                });
            });
        }

        /// <summary>
        /// Este método é chamado pelo runtime. Use este método para configurar o pipeline de requisições HTTP.
        /// </summary>
        /// <param name="app">O construtor do pipeline de requisições HTTP.</param>
        /// <param name="env">O ambiente de hospedagem da aplicação.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Mostra uma página de erro detalhada em ambiente de desenvolvimento
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Usa uma página de erro genérica e HSTS (HTTP Strict Transport Security) em produção
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Redireciona HTTP para HTTPS
            app.UseHttpsRedirection();
            // Permite o uso de arquivos estáticos (como CSS e JavaScript)
            app.UseStaticFiles();
            // Configura o roteamento de requisições
            app.UseRouting();

            // Configuração do Swagger para a interface de documentação
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // Define o endpoint do Swagger para acesso à documentação
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API CrmEducacional v1");
                c.RoutePrefix = string.Empty; // Define a URL raiz para acessar o Swagger UI
                // c.RoutePrefix = "swagger"; // Alternativamente, define um prefixo para o Swagger UI
            });

            // Configura os endpoints da aplicação
            app.UseEndpoints(endpoints =>
            {
                /*
                // Rota para exibir uma lista de rotas disponíveis (comentada como exemplo)
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                */

                // Rota padrão (fallback) que mapeia o controlador e a ação padrão
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Mapeia todos os controllers automaticamente
                endpoints.MapControllers();
            });
        }
    }
}
