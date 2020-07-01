// (c) California State University, Fullerton

namespace Csuf.ApiTemplate
{
	using System;
	using System.Security.Claims;
	using System.Security.Principal;

	public static class IIdentityExtensions
	{
		public static string GetNameOrSub(this IIdentity source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			Claim iss = null;
			Claim sub = null;
			Claim name = null;
			if (source is ClaimsIdentity identity)
			{
				iss = identity.FindFirst("iss");
				sub = identity.FindFirst("sub");
				name = identity.FindFirst("name") ?? (_ = identity.FindFirst("unique_name") ?? sub);
			}
			if (iss == null || sub == null || name == null || string.IsNullOrWhiteSpace(iss.Value) || string.IsNullOrWhiteSpace(sub.Value) || string.IsNullOrWhiteSpace(name.Value)) return null;
			return name.Value;
		}

		public static string GetCwid(this IIdentity source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			Claim cwidClaim = null;
			if (source is ClaimsIdentity identity)
			{
				cwidClaim = identity.FindFirst("cwid");
				if (cwidClaim == null || string.IsNullOrWhiteSpace(cwidClaim.Value))
				{
					Claim sub = identity.FindFirst("sub");
					if (sub != null && !string.IsNullOrWhiteSpace(sub.Value)) return sub.Value;
				};
			}
			return cwidClaim?.Value;
		}

		public static string GetEmail(this IIdentity source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			Claim emailClaim = null;
			if (source is ClaimsIdentity identity)
			{
				emailClaim = identity.FindFirst("email");
				if (emailClaim == null || string.IsNullOrWhiteSpace(emailClaim.Value))
				{
					Claim sub = identity.FindFirst("sub");
					if (sub != null && !string.IsNullOrWhiteSpace(sub.Value)) return sub.Value;
				};
			}
			return emailClaim?.Value;
		}
	}
}