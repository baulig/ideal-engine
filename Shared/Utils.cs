//
// Utils.cs
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
using System.Security.Cryptography.X509Certificates;
using Mono.Security.Interface;

namespace Shared {
	public static class Utils {
		public static byte [] ReadResource (string name)
		{
			var asm = Assembly.GetExecutingAssembly ();
			using (var stream = asm.GetManifestResourceStream (name)) {
				var buffer = new byte [stream.Length];
				var ret = stream.Read (buffer, 0, buffer.Length);
				if (ret != buffer.Length)
					throw new IOException ();
				return buffer;
			}
		}

		public static void Initialize ()
		{
			MonoTlsProviderFactory.Initialize ();

			var trustAnchors = new X509CertificateCollection ();

			var caCertData = Utils.ReadResource ("Hamiller-Tube-CA.pem");
			var imCertData = Utils.ReadResource ("Hamiller-Tube-IM.pem");
			trustAnchors.Add (new X509Certificate (caCertData));
			trustAnchors.Add (new X509Certificate (imCertData));

			MonoTlsSettings.DefaultSettings.TrustAnchors = trustAnchors;
		}
	}
}
