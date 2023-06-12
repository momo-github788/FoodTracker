namespace SuperHeroApi.Configuration {

    // We use this to be able to register the GlobalErrorHandler as a middleware in Program.cs
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder app) =>
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
