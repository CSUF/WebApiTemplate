// (c) California State University, Fullerton.  All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace Csuf.ApiTemplate.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PeriodController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public PeriodController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		public async Task<DataTable> GetAllPeriods()
		{
			DataTable result = new DataTable();
			string sqlCommandText = "SELECT TOP 5 [t].[Description] [TermName] FROM [dbo].[view_tblTerm] [t] ORDER BY [t].[TermCode] DESC";
			using SqlConnection theSqlConn = new SqlConnection(connectionString: _configuration.GetConnectionString("PersonDbContext"));
			SqlDataAdapter theDataAdapter = new SqlDataAdapter(selectCommandText: sqlCommandText, selectConnection: theSqlConn);
			theDataAdapter.Fill(dataTable: result);
			return await Task.FromResult(result);
		}
	}
}