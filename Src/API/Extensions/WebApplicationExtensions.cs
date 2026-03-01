namespace API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

           
            app.MapControllers();
 

            return app;
        }
    }
}
