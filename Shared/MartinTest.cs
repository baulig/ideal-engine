﻿//
// MartinTest.cs
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
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Shared {
	public static class MartinTest {
		public static async Task Run ()
		{
			await TestHttpClient ();
		}

		static async Task TestClientServer ()
		{
			var serverTask = Server.Run ();
			var clientTask = Client.Run ();

			while (!clientTask.IsCompleted || !serverTask.IsCompleted) {
				await Task.WhenAny (clientTask, serverTask).ConfigureAwait (false);

				if (serverTask.IsCompleted)
					await serverTask;
				if (clientTask.IsCompleted)
					await clientTask;
			}
		}

		public static async Task TestHttpClient ()
		{
			var client = new HttpClient ();
			client.Timeout = TimeSpan.FromSeconds (150);

			var watch = new Stopwatch ();
			watch.Start ();

			using (var cts = new CancellationTokenSource ()) {
				// cts.CancelAfter (100);
				await client.GetAsync ("https://httpstat.us/200?sleep=500000", cts.Token);
			}

			watch.Stop ();

			Console.Error.WriteLine ($"HTTP CLIENT DONE: {watch.ElapsedMilliseconds}");
			Console.Error.WriteLine ();
		}
	}
}
