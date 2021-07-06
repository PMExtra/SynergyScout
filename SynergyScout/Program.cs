using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SynergyScout
{
    public static class Program
    {
        private static readonly HttpClient Http;

        private static readonly Dictionary<string, string> OSList = new Dictionary<string, string>
        {
            {"windows", "msi"},
            {"macos", "dmg"},
            {"ubuntu18", "deb"},
            {"ubuntu19", "deb"},
            {"ubuntu20", "deb"},
            {"debian9", "deb"},
            {"debian10", "deb"}
        };

        private static readonly string[] PlatformList =
        {
            "amd64",
            "x64",
            "x86_64",
            "x86-64"
        };

        static Program()
        {
            Http = new HttpClient
            {
                BaseAddress = new Uri("https://binaries.symless.com/synergy/v1-core-standard/")
            };
        }

        private static async Task<string> FindAsync(string os, string version)
        {
            foreach (var platform in PlatformList)
            {
                var path = $"{version}/synergy_{version}_{os}_{platform}.{OSList[os]}";
                using var request = new HttpRequestMessage(HttpMethod.Head, path);
                using var response = await Http.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return request.RequestUri!.ToString();
                }
            }

            return null;
        }

        public static async Task Main(string[] args)
        {
            const string version = "1.14.0-stable.67d824b8";

            foreach (var os in OSList.Keys)
            {
                var uri = await FindAsync(os, version);
                Console.WriteLine(uri ?? $"Cannot find the distribute of {os}.");
            }
        }
    }
}