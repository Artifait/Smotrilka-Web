using Smotrilka_Web.Services;

namespace Smotrilka_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient<BackendService>(client =>
            {
                client.BaseAddress = new Uri("http://158.160.120.54:9090");
                //client.BaseAddress = new Uri("http://127.0.0.1:9090");
            });

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapRazorPages();

            app.Run();
        }
    }
}