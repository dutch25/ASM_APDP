namespace ASM_APDP.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Allow access to the Index page without authentication
            if (context.Request.Path == "/" || context.Request.Path == "/Home/Index" || context.Request.Path == "/User/Index")
            {
                await _next(context);
                return;
            }

            // Redirect to the login page if not authenticated
            if (string.IsNullOrEmpty(context.Session.GetString("username")))
            {
                context.Response.Redirect("/User/Login");
                return;
            }

            await _next(context);
        }
    }
}
