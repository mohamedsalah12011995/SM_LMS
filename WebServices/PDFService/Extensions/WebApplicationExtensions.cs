namespace PDFService.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureSwagger(this WebApplication app)
        {
          
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RM.PDFService v1"));
            }
            return app;
        }

    }
}
