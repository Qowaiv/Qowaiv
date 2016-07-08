namespace Qowaiv.Financial
{
	internal partial struct CountryToCurrency
	{
		private Country m_Country;
		private Currency m_Currency;
		private Date m_StartDate;
		

		public CountryToCurrency(Country country, Currency currency, Date startdate)
		{
			m_Country = country;
			m_Currency = currency;
			m_StartDate = startdate;
		}

		public CountryToCurrency(Country country, Currency currency) 
			: this(country, currency, Date.MinValue) { }

		public Country Country { get { return m_Country; } }
		public Currency Currency { get { return m_Currency; } }
		public Date StartDate { get { return m_StartDate; } }
	}
}
