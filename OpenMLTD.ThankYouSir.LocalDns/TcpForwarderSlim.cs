using System;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using OpenMLTD.ThankYouSir.Core;

namespace OpenMLTD.ThankYouSir.LocalDns {
    /// <summary>
    /// A simple TCP forwarder.
    /// </summary>
    /// <remarks>
    /// See http://blog.brunogarcia.com/2012/10/simple-tcp-forwarder-in-c.html.
    /// </remarks>
    internal sealed class TcpForwarderSlim : DisposableBase {

        public TcpForwarderSlim() {
            MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public Socket MainSocket { get; }

        public void Start([NotNull] Lazy<IPEndPoint> localFunc, [NotNull] Lazy<IPEndPoint> remoteFunc) {
            var remote = remoteFunc.Value;
            var local = localFunc.Value;
            try {
                MainSocket.Bind(local);
                MainSocket.Listen(10);
                while (true) {
//                    Console.WriteLine("{0} : Starting forwarding with new tunnel", remote.Address);
                    var source = MainSocket.Accept();
                    var destination = new TcpForwarderSlim();
                    var state = new State(source, destination.MainSocket);
                    destination.Connect(remote, source);
                    source.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceive, state);
//                    Console.WriteLine("{0} : Request Forwarded", remote.Address);
                }
            } catch (SocketException) {
//                Console.WriteLine("{0} :  Closing tunnel", remote.Address);
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                MainSocket.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Connect([NotNull] EndPoint remoteEndPoint, [NotNull] Socket destination) {
            var state = new State(MainSocket, destination);
            MainSocket.Connect(remoteEndPoint);
            MainSocket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, OnDataReceive, state);
        }

        private static void OnDataReceive(IAsyncResult result) {
            var state = (State)result.AsyncState;
            try {
                var bytesRead = state.SourceSocket.EndReceive(result);
                if (bytesRead > 0) {
                    state.DestinationSocket.Send(state.Buffer, bytesRead, SocketFlags.None);
                    state.SourceSocket.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceive, state);
                }
            } catch {
                state.DestinationSocket.Close();
                state.SourceSocket.Close();
            }
        }

        private sealed class State {

            public State([NotNull] Socket source, [NotNull] Socket destination) {
                SourceSocket = source;
                DestinationSocket = destination;
                Buffer = new byte[8192];
            }

            [NotNull]
            public Socket SourceSocket { get; }

            [NotNull]
            public Socket DestinationSocket { get; }

            [NotNull]
            public byte[] Buffer { get; }

        }

    }
}