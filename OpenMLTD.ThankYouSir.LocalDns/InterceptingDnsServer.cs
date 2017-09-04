using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ARSoft.Tools.Net.Dns;
using JetBrains.Annotations;

namespace OpenMLTD.ThankYouSir.LocalDns {
    public class InterceptingDnsServer {

        public InterceptingDnsServer([NotNull] IPAddress endDnsIP, [NotNull] IPAddress serverIP, [NotNull, ItemNotNull] IReadOnlyList<Regex> redirectPatterns)
            : this(endDnsIP, serverIP, redirectPatterns, null) {
        }

        public InterceptingDnsServer([NotNull] IPAddress endDnsIP, [NotNull] IPAddress serverIP, [NotNull, ItemNotNull] IReadOnlyList<Regex> redirectPatterns, [CanBeNull] PortForwardingManager manager) {
            _endDnsIP = endDnsIP;
            _serverIP = serverIP;
            _redirectPatterns = redirectPatterns;

            _state = DnsServerState.Stopped;

            _portForwardingManager = manager;
        }

        ~InterceptingDnsServer() {
            if (IsRunning) {
                Stop();
            }
        }

        public void Start() {
            if (_state == DnsServerState.NotInitialized) {
                throw new InvalidOperationException("You have to initialize InterceptingDnsServer before using it.");
            }

            if (IsRunning) {
                return;
            }

            var dnsEndPoint = new IPEndPoint(IPAddress.Any, 53);
            var dnsServer = new DnsServer(dnsEndPoint, 10, 10);
            _dnsServer = dnsServer;
            dnsServer.QueryReceived += OnQueryReceived;
            dnsServer.Start();

            _state = DnsServerState.Running;
        }

        public void Stop() {
            if (!IsRunning) {
                return;
            }

            if (_dnsServer != null) {
                _dnsServer.Stop();
                _dnsServer.QueryReceived -= OnQueryReceived;
            }

            _state = DnsServerState.Stopped;
        }

        public bool IsRunning => _state == DnsServerState.Running;

        protected virtual bool IsAccessAllowed([NotNull] IPEndPoint clientEndPoint, [NotNull] DnsQuestion question) {
            return true;
        }

        private Task OnQueryReceived(object sender, QueryReceivedEventArgs e) {
            var query = (DnsMessage)e.Query;
            query.IsQuery = false;

            var question = query.Questions.First();
            var clientEndPoint = e.RemoteEndpoint;
            var accessAllowed = IsAccessAllowed(clientEndPoint, question);
            if (!accessAllowed) {
                return Task.FromResult(1);
            }

            var answer = ResolveDnsQuery(question);

            if (IsRedicted(question)) {
                // Resolved with redirection

                SetupReponse(query, question);

                if (_portForwardingManager != null) {
                    SetupForwarding(answer);
                }

                e.Response = query;
                return Task.FromResult(0);
            }

            if (query.Questions.Count == 1 && answer != null) {
                foreach (var record in answer.AnswerRecords) {
                    query.AnswerRecords.Add(record);
                }
                foreach (var record in answer.AdditionalRecords) {
                    query.AnswerRecords.Add(record);
                }

                // Resolved without redirection

                query.ReturnCode = ReturnCode.NoError;

                e.Response = query;
                return Task.FromResult(0);
            }

            // Failed to resolve ...
            query.ReturnCode = ReturnCode.ServerFailure;
            e.Response = query;

            return Task.FromResult(0);
        }

        private DnsMessage ResolveDnsQuery([NotNull] DnsQuestion question) {
            var ips = new[] { _endDnsIP };
            var client = new DnsClient(ips, (int)NormalDnsQueryTimeout.TotalMilliseconds);
            var answer = client.Resolve(question.Name, question.RecordType, question.RecordClass);

            if (answer == null) {
                // Failed...
            }

            return answer;
        }

        private bool IsRedicted([NotNull] DnsQuestion question) {
            if (_redirectPatterns.Count == 0) {
                return question.RecordType == RecordType.A;
            }

            var domainName = question.Name.ToString();
            return _redirectPatterns.Any(re => re.IsMatch(domainName));
        }

        private void SetupReponse([NotNull] DnsMessage query, [NotNull] DnsQuestion question) {
            var record = new ARecord(question.Name, 100, _serverIP);
            query.AnswerRecords.Add(record);
            query.ReturnCode = ReturnCode.NoError;
        }

        private void SetupForwarding([NotNull] DnsMessage answer) {
            if (_portForwardingManager == null) {
                throw new InvalidOperationException();
            }

            var response = answer?.AnswerRecords.OfType<ARecord>().FirstOrDefault();

            if (response == null || !response.Address.Equals(_redirectIP)) {
                return;
            }

            Console.WriteLine("{0} : Found a record to proxy with https", response.Name);

            _redirectIP = response.Address;
            var ep = new IPEndPoint(_redirectIP, HttpsPort);
            _portForwardingManager.StartForwarding(ep);
        }

        public const int HttpsPort = 443;

        private static readonly TimeSpan NormalDnsQueryTimeout = TimeSpan.FromSeconds(8);

        private readonly IPAddress _endDnsIP;
        private readonly IPAddress _serverIP;
        private IPAddress _redirectIP;

        [NotNull, ItemNotNull]
        private IReadOnlyList<Regex> _redirectPatterns;

        [CanBeNull]
        private readonly PortForwardingManager _portForwardingManager;

        [CanBeNull]
        private DnsServer _dnsServer;

        private DnsServerState _state;

        private enum DnsServerState {

            NotInitialized,
            Stopped,
            Running

        }

    }
}