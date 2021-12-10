using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerHost
{
    internal class HttpServer
    {
        private HttpListener listener;

        private const string PREFIX = "http://127.0.0.1:8214/";

        public HttpServer()
        {
            listener = new();
            listener.Prefixes.Add(PREFIX);
        }

        public void Start()
        {
            listener.Start();
            StartWait();
        }

        public void Stop()
        {
            listener.Close();
        }

        public event EventHandler<IncomingHttpRequestEventArgs>? IncomingHttpRequest;

        private void StartWait()
        {
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            HttpListener? listener = result.AsyncState as HttpListener;
            if (listener == null) return;
            HttpListenerContext context = listener.EndGetContext(result);
            StartWait();

            if (IncomingHttpRequest != null)
                IncomingHttpRequest(this, new(context.Request, context.Response));
        }
    }

    public class IncomingHttpRequestEventArgs : EventArgs
    {
        public IncomingHttpRequestEventArgs(HttpListenerRequest request, HttpListenerResponse response)
        {
            Request = request;
            Response = response;
        }

        public HttpListenerRequest Request { get; set; }

        public HttpListenerResponse Response { get; set; }
    }
}
