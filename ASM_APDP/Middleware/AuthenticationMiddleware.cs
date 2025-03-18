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
            if(context.Request.Path == "/" || context.Request.Path == "/Home/Index")
            {
                if (string.IsNullOrEmpty(context.Session.GetString("username"))) { 
                    context.Response.Redirect("/User/Login");
                }
            }
            await _next(context);
        }
    }
}
