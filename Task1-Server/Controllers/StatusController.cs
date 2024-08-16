using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace EquipmentWatcher.Controllers
{
	[ApiController]
	[Route("api/status")]
	public class StatusController : ControllerBase
	{
		[HttpGet]
		public IActionResult GetStatus()
		{
			var serverStatus = "Server is alive";
			var uptime = DateTime.Now - Process.GetCurrentProcess().StartTime;
			var activeTime = uptime.ToString();
			var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			var statusResponse = new
			{
				ServerStatus = serverStatus,
				ActiveTime = activeTime,
				CurrentTime = currentTime
			};

			return Ok(statusResponse);
		}
	}
}
