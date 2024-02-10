Console.WriteLine("soner1 Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
Task q = FetchAndShowHeaders("http://www.fultonbank.com");
Console.WriteLine("soner2 Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
for (int i = 0; i < 500_000_0; i++)
{
	for (int ii = 0; ii < 500; ii++)
	{
		if (i % 500_000 == 0)
			Console.Write(".");
	}
}
Console.WriteLine("\nsoner5 Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
await q;
Console.WriteLine("soner6 Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
return;

async Task FetchAndShowHeaders(string url)
{
	using var w = new HttpClient();
	var req = new HttpRequestMessage(HttpMethod.Head, url);
	Console.WriteLine("\nsoner3_method_inside Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
	var response = await w.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
	Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(
		response.Headers.ToString()));
		Console.WriteLine("\nsoner4_method_inside Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
}

/*
soner1 Thread Name = 1

soner3_method_inside Thread Name = 1
soner2 Thread Name = 1
......................................................................."Date: Sat, 10 Feb 2024 21:08:01 GMT\r\nConnection: keep-alive\r\nSet-Cookie: AWSALB=yoEkz4GX4D6NZxccG55ShbBWdBKOo29cVFdYVQqsyWzdLpmv4gs6hsWeraPs1OQfQGu0yNRocZplmI6TE1pU1GBkGPGr8R345oShAkOc4RIci84X7t\u002BNwAcQ01\u002Bc; Expires=Sat, 17 Feb 2024 21:08:01 GMT; Path=/, AWSALBCORS=yoEkz4GX4D6NZxccG55ShbBWdBKOo29cVFdYVQqsyWzdLpmv4gs6hsWeraPs1OQfQGu0yNRocZplmI6TE1pU1GBkGPGr8R345oShAkOc4RIci84X7t\u002BNwAcQ01\u002Bc; Expires=Sat, 17 Feb 2024 21:08:01 GMT; Path=/; SameSite=None; Secure, ASP.NET_SessionId=al3ekazz02q0up1oja4i0ify; path=/; HttpOnly; SameSite=Lax, ASP.NET_SessionId=al3ekazz02q0up1oja4i0ify; path=/; HttpOnly; SameSite=Lax, Fulton.Foundation.ContactIdentification.Cookies.ContactIdentificationCookieManager=ModelData={\u0022DoNotTrack\u0022:true,\u0022Retry\u0022:false,\u0022Message\u0022:\u0022Contact is affected by Gdpr.\u0022}; path=/, SC_ANALYTICS_GLOBAL_COOKIE=81142a4520a2492c97f5331ed0e7cb27|False; domain=www.fultonbank.com; expires=Tue, 07-Feb-2034 21:08:01 GMT; path=/; secure; HttpOnly; SameSite=None, __RequestVerificationToken=pXsvPUEZfHmlsSXJ9eecbM18Mk9QNpyYqbASbEvP3UyYGKymOLDJS1Mxi_4_IaYD38AvtOi0Y6l-zLzsmi8XkYapo6j9_wz0FGs3t1Ll6iI1; path=/; HttpOnly, Fulton.Foundation.ContactIdentification.Cookies.ContactIdentificationCookieManager=ModelData={\u0022DoNotTrack\u0022:true,\u0022Retry\u0022:false,\u0022Message\u0022:\u0022Contact is affected by Gdpr.\u0022}; path=/\r\nCache-Control: no-cache, no-store\r\nPragma: no-cache\r\nX-Frame-Options: DENY\r\nStrict-Transport-Security: max-age=31536000;includeSubDomains\r\nX-Content-Type-Options: nosniff\r\nX-XSS-Protection: 1; mode=block\r\n"

soner4_method_inside Thread Name = 10
........................................................................................................................................................................................................................................................................
........................................................................................................................................................................................................................................................................
........................................................................................................................................................................................................................................................................
soner5 Thread Name = 1
soner6 Thread Name = 1
*/


//veya

Console.WriteLine("soner1 Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
Task q = FetchAndShowHeaders("http://www.fultonbank.com");
Console.WriteLine("soner2 Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
for (int i = 0; i < 11; i++)
{
	for (int ii = 0; ii < 11; ii++)
	{
		//if (i % 500_000 == 0)
			Console.Write(".");
	}
}
Console.WriteLine("\nsoner5 Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
await q;
Console.WriteLine("soner6 Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
return;

async Task FetchAndShowHeaders(string url)
{
	Console.WriteLine("\nsoner3_method_inside Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
		await Task.Delay(1000);
		Console.WriteLine("\nsoner4_method_inside Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
}

soner1 Thread Name = 1

soner3_method_inside Thread Name = 1
soner2 Thread Name = 1
.........................................................................................................................
soner5 Thread Name = 1

soner4_method_inside Thread Name = 7
soner6 Thread Name = 7
