using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qowaiv.Financial
{

	[Serializable]
	public class CurrencyMismatchException : Exception
	{
		private Currency currency1;
		private Currency currency2;
		private string operation;

		public CurrencyMismatchException() { }
		public CurrencyMismatchException(string message) : base(message) { }
		public CurrencyMismatchException(string message, Exception inner) : base(message, inner) { }

		public CurrencyMismatchException(Currency currency1, Currency currency2, string operation)
		{
			this.currency1 = currency1;
			this.currency2 = currency2;
			this.operation = operation;
		}

		protected CurrencyMismatchException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
