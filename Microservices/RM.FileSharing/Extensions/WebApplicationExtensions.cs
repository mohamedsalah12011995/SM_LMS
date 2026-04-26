namespace RM.FileSharing.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureSwagger(this WebApplication app)
        {
          
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RM.FileSharing v1"));
            }
            return app;
        }


        public static WebApplication ConfigureDirectoryResources(this WebApplication app)
        {

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"Resources"));

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/Resources", @"images")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory() + "/Resources", @"images"));

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/Resources", @"icons")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory() + "/Resources", @"icons"));

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/Resources", @"files")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory() + "/Resources", @"files"));

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory() + "/Resources", @"videos")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory() + "/Resources", @"videos"));

            return app;
        }

    }
}
