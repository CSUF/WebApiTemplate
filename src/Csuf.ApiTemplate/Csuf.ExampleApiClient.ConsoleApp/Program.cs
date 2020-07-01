// (c) California State University, Fullerton.  All rights reserved.

using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Csuf.ExampleApiClient.ConsoleApp
{
	public class TermModel
	{
		public string TermName { get; set; }
	}

	public class Program
	{
		public static IConfiguration _configuration { get; set; }

		private static async Task Main(string[] args)
		{
			// Setup the configuration
			IConfigurationBuilder builder = new ConfigurationBuilder();
			string devEnvironmentVariable = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
			var isDevelopment = string.IsNullOrWhiteSpace(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";
			if (isDevelopment) builder.AddUserSecrets<Program>();
			builder.SetBasePath(Directory.GetCurrentDirectory())
						 .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
						 .AddEnvironmentVariables();
			_configuration = builder.Build();
			if (builder == null)
			{
				throw new Exception("Missing or invalid appsettings.json file. Please see README.md for configuration instructions.");
			}

			// Setup HttpClient
			using HttpClient _theClient = new HttpClient() { BaseAddress = new Uri(_configuration["ExampleApiClient:ApiUrl"], UriKind.Absolute) };
			_theClient.DefaultRequestHeaders.Accept.Clear();
			_theClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			// Request bearer token / access token and set it
			var accessTokenResponse = await _theClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
			{
				Address = _configuration["ExampleApiClient:ApiAuthorityUrlTokenEndpoint"],
				ClientId = _configuration["ExampleApiClient:ApiClientId"],
				ClientSecret = _configuration["ExampleApiClient:ApiClientSecret"],
				Scope = "sampleApi"
			});
			_theClient.SetBearerToken(accessTokenResponse.AccessToken);

			// Call the API and display result
			try
			{
				List<TermModel> apiResult = await _theClient.GetFromJsonAsync<List<TermModel>>(new Uri(_configuration["ExampleApiClient:ApiRoute"], UriKind.Relative));
				apiResult.ForEach(x => Console.WriteLine(x.TermName));
			}
			catch (Exception ex)
			{
				Console.WriteLine($"API Call Failure: {ex.GetType().Name}: {ex.Message}");
			}
		}
	}
}