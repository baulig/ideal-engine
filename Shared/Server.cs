﻿//
// Server.cs
//
// Author:
//       Martin Baulig <mabaul@microsoft.com>
//
// Copyright (c) 2019 Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Shared {
	public static class Server {
		public static async Task Run ()
		{
			var listener = new TcpListener (new IPEndPoint (IPAddress.Loopback, 9999));
			listener.Start ();

			Console.WriteLine ($"SERVER LISTENING");

			var socket = await listener.AcceptSocketAsync ().ConfigureAwait (false);
			Console.WriteLine ($"ACCEPTED CONNECTION FROM {socket.RemoteEndPoint}");

			var ns = new NetworkStream (socket);
			var stream = new SslStream (ns);

			var certData = Utils.ReadResource ("server-cert-im-full.pfx");
			var cert = new X509Certificate2 (certData, "monkey");

			await stream.AuthenticateAsServerAsync (cert);

			Console.WriteLine ("SERVER AUTH OK");
		}
	}
}
