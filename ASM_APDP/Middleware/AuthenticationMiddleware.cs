namespace ASM_APDP.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if ((context.Session.GetInt32("IsLogin") != 1 || context.Session.GetString("Username") == ""
                || context.Session.GetString("Username") == null || context.Session.GetInt32("IsLogin") == null)
                && !context.Request.Path.StartsWithSegments("/User"))
            {
                context.Response.Redirect("/User/Login");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
