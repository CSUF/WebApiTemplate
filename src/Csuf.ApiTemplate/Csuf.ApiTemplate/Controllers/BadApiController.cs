// (c) California State University, Fullerton.  All rights reserved.

using Microsoft.AspNetCore.Mvc;
using System;

namespace Csuf.ApiTemplate.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class BadApiController : ControllerBase
	{
		[HttpGet]

		public void Index()
		{
			throw new InvalidOperationException();
		}

	}
}