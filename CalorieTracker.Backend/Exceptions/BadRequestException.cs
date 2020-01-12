using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Backend.Exceptions
{
	/// <summary>
	/// Generates response 400 (BadRequest).
	/// </summary>
	[Serializable]
	public class BadRequestException : Exception
	{
		public BadRequestException() { }
		public BadRequestException(string message) : base(message) { }
		public BadRequestException(string message, Exception inner) : base(message, inner) { }
		protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
