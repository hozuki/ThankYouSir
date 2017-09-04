using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy.Network;

namespace OpenMLTD.ThankYouSir.Akachan {
    public sealed class Test {

        public void Start() {
            _proxyServer = new ProxyServer();

            // locally trust root certificate used by this proxy 
            _proxyServer.TrustRootCertificate = true;

            //optionally set the Certificate Engine
            //Under Mono only BouncyCastle will be supported
            _proxyServer.CertificateEngine = CertificateEngine.BouncyCastle;

            _proxyServer.BeforeRequest += OnRequest;
            _proxyServer.BeforeResponse += OnResponse;
            _proxyServer.ServerCertificateValidationCallback += OnCertificateValidation;
            _proxyServer.ClientCertificateSelectionCallback += OnCertificateSelection;

            var startupPath = Environment.CurrentDirectory;
            var certPath = Path.Combine(startupPath, "FiddlerRoot.cer");
            var cert = new X509Certificate2(certPath);

            var ep1 = new TransparentProxyEndPoint(IPAddress.Any, 80, true) {
                //Use self-issued generic certificate on all HTTPS requests
                //Optimizes performance by not creating a certificate for each HTTPS-enabled domain
                //Useful when certificate trust is not required by proxy clients
//                GenericCertificate = cert
            };
            var ep2 = new TransparentProxyEndPoint(IPAddress.Any, 443, true) {
//                GenericCertificate = cert
            };

            //An explicit endpoint is where the client knows about the existence of a proxy
            //So client sends request in a proxy friendly manner
            _proxyServer.AddEndPoint(ep1);
            _proxyServer.AddEndPoint(ep2);

            _proxyServer.Enable100ContinueBehaviour = true;

            foreach (var endPoint in _proxyServer.ProxyEndPoints) {
                Console.WriteLine("Listening on '{0}' endpoint at Ip {1} and port: {2} ", endPoint.GetType().Name, endPoint.IpAddress, endPoint.Port);
            }

            _proxyServer.Start();

//            _proxyServer.SetAsSystemProxy(ep1, ProxyProtocolType.AllHttp);
        }

        public void Stop() {
            //Unsubscribe & Quit
            _proxyServer.BeforeRequest -= OnRequest;
            _proxyServer.BeforeResponse -= OnResponse;
            _proxyServer.ServerCertificateValidationCallback -= OnCertificateValidation;
            _proxyServer.ClientCertificateSelectionCallback -= OnCertificateSelection;

            _proxyServer.Stop();
        }

        public Task OnRequest(object sender, SessionEventArgs e) {
            Console.WriteLine("> Request: {0} {1}", e.WebSession.Request.Method, e.WebSession.Request.Url);

            return Task.FromResult(0);
        }

        //Modify response
        private async Task OnResponse(object sender, SessionEventArgs e) {
            Console.WriteLine("> Response: {0}", e.WebSession.Request.Url);

            var method = e.WebSession.Request.Method;
            if (method.ToLowerInvariant() == "head") {
                Console.WriteLine("head!");
            }

//            var str = await e.GetResponseBodyAsString();
//            Console.WriteLine(str);

//            var bytes = await e.GetResponseBody();
//            var headers = e.WebSession.Response.ResponseHeaders;
//            var h2 = headers.ToDictionary(kv => kv.Name, kv => new HttpHeader(kv.Name, kv.Value));
//            await e.Ok(bytes, h2);

//            return Task.FromResult(0);
        }

        /// Allows overriding default certificate validation logic
        private Task OnCertificateValidation(object sender, CertificateValidationEventArgs e) {
            //set IsValid to true/false based on Certificate Errors
            if (e.SslPolicyErrors == System.Net.Security.SslPolicyErrors.None) {
                e.IsValid = true;
            }

            return Task.FromResult(0);
        }

        /// Allows overriding default client certificate selection logic during mutual authentication
        private Task OnCertificateSelection(object sender, CertificateSelectionEventArgs e) {
            //set e.clientCertificate to override
            return Task.FromResult(0);
        }

        private ProxyServer _proxyServer;

    }
}