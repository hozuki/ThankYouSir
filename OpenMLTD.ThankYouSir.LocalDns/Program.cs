using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using OpenMLTD.ThankYouSir.Logging;

namespace OpenMLTD.ThankYouSir.LocalDns {
    internal static class Program {

        [STAThread]
        private static void Main(string[] args) {
            if (args.Length == 1) {
                var arg0 = args[0].ToLowerInvariant();
                switch (arg0) {
                    case "-i":
                    case "/i":
                        PrintIPInfo();
                        return;
                    case "-h":
                    case "-?":
                    case "/h":
                    case "/?":
                    case "/help":
                    case "--help":
                        PrintUsage(false);
                        return;
                    default:
                        PrintUsage(false);
                        return;
                }
            }

            Ddf.LoggingEnabled = true;

            var configFile = new FileInfo(DnsConfigFilePath);
            if (!configFile.Exists) {
                PrintUsage(true);
                return;
            }

            DnsConfig dnsConfig;
            try {
                var jsonData = File.ReadAllText(DnsConfigFilePath);
                dnsConfig = JsonConvert.DeserializeObject<DnsConfig>(jsonData);
            } catch (Exception) {
                PrintUsage(true);
                return;
            }

            var patterns = new Regex[dnsConfig.RedirectPatterns.Length];
            for (var i = 0; i < patterns.Length; ++i) {
                patterns[i] = new Regex(dnsConfig.RedirectPatterns[i], RegexOptions.CultureInvariant);
            }

            Console.TreatControlCAsInput = true;
            Console.CancelKeyPress += OnConsoleCancelKeyPressed;

            var localIP = IPAddress.Parse(dnsConfig.LocalIP);
            if (!CheckIP(localIP)) {
                Ddf.InfoFormat(Lang.Get("ip_not_found+tpl"), localIP);
                return;
            }

            StartDns(localIP, patterns);
            Ddf.InfoFormat(Lang.Get("dns_started+tpl"), localIP);

            while (true) {
                var key = Console.ReadKey(true);
                if ((key.Key == ConsoleKey.C && key.Modifiers == ConsoleModifiers.Control) || key.Key == ConsoleKey.Enter) {
                    break;
                }
            }

            StopDns();
            Ddf.Info(Lang.Get("dns_stopped"));

            Console.CancelKeyPress -= OnConsoleCancelKeyPressed;
        }

        private static void PrintIPInfo() {
            var ss = IPAddressHelper.GetAvailableIPAddresses();
            foreach (var s in ss) {
                Console.WriteLine("{0} ({1})", s.Key, s.Value);
            }
        }

        private static void PrintUsage(bool wait) {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var usage =
                $@"
Usage:
        {assemblyName.Name}.exe [options]

Options:

        -h --help    Display this message.
        -i           Display a list of machine IP addresses.

    When started without arguments, the program tries to load configuration
    from `{DnsConfigFilePath}`.
    If the configuration file is not found or any error occurs while loading,
    this message is displayed.";
            Console.WriteLine(usage);

            if (wait) {
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private static void OnConsoleCancelKeyPressed(object sender, ConsoleCancelEventArgs e) {
            e.Cancel = true;
            Console.WriteLine("Please press Enter or Ctrl-C to stop the proxy.");
        }

        private static bool CheckIP([NotNull] IPAddress localIP) {
            var ss = IPAddressHelper.GetAvailableIPAddresses();
            return ss.Any(s => s.Key.Equals(localIP));
        }

        private static void StartDns([NotNull] IPAddress localIP, [NotNull] IReadOnlyList<Regex> patterns) {
            if (_interceptingDnsServer != null) {
                return;
            }
            var remoteDns = IPAddress.Parse("8.8.8.8");
            _forwardingManager = new PortForwardingManager();
            _interceptingDnsServer = new InterceptingDnsServer(localIP, remoteDns, patterns, _forwardingManager);
            _interceptingDnsServer.Start();
        }

        private static void StopDns() {
            if (_interceptingDnsServer == null) {
                return;
            }
            _interceptingDnsServer.Stop();
            _interceptingDnsServer = null;
            _forwardingManager = null;
        }

        private static readonly string DnsConfigFilePath = "Resources/Config/dns.json";

        private static InterceptingDnsServer _interceptingDnsServer;
        private static PortForwardingManager _forwardingManager;

    }
}