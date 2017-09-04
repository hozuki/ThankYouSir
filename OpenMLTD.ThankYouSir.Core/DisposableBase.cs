using System;

namespace OpenMLTD.ThankYouSir.Core {
    public abstract class DisposableBase : IDisposable {

        /// <summary>
        /// Dispose the instance.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> means it is called in <see cref="Dispose"/>, <see langword="false"/> means it is called in <see cref="Finalize"/></param>
        protected virtual void Dispose(bool disposing) {
        }

        /// <summary>
        /// Dispose the instance.
        /// </summary>
        public void Dispose() {
            if (IsDisposed) {
                return;
            }
            Dispose(true);
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        public bool IsDisposed {
            get {
                lock (_disposeLock) {
                    return _isDisposed;
                }
            }
            private set {
                lock (_disposeLock) {
                    _isDisposed = value;
                }
            }
        }

        protected void EnsureNotDisposed() {
            if (IsDisposed) {
                throw new ObjectDisposedException(ToString());
            }
        }

        ~DisposableBase() {
            if (IsDisposed) {
                return;
            }
            Dispose(false);
            IsDisposed = true;
        }

        private bool _isDisposed;

        private readonly object _disposeLock = new object();

    }
}