using System;

namespace OpenMLTD.ThankYouSir.Akachan {
    internal static class Program {

        private static void Main(string[] args) {
            var test = new Test();
            test.Start();
            Console.Read();
            test.Stop();
        }

    }
}