namespace RM.GateWay.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RM.GatewayInner v1"));
            }
           
            return app;
        }
        public static WebApplication ConfigureEndpoints(this WebApplication app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World! gateway");
                });
            });

            return app;
        }
    }
}
