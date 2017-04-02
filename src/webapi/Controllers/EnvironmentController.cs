using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;


namespace webapi.Controllers
{
    [Route("[controller]")]
    public class EnvironmentController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var envInfo = new Models.Environment(
                Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion,
                Environment.MachineName);

            return base.Ok(envInfo);
        }
    }
}
