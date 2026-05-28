namespace Cupid.Models
{
    public class Letter
    {
		private string fromUsername;

		public string FromUsername
		{
			get { return fromUsername; }
			set { fromUsername = value; }
		}

		private string city;

		public string City
		{
			get { return city; }
			set { city = value; }
		}

		private int age;

		public int Age
		{
			get { return age; }
			set { age = value; }
		}

		private string message;

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		private string phoneNumber;

		public string PhoneNumber
		{
			get { return phoneNumber; }
			set { phoneNumber = value; }
		}

        public Letter(string fromUsername, string city, int age, string message, string phoneNumber)
        {
            this.fromUsername = fromUsername;
            this.city = city;
            this.age = age;
            this.message = message;
            this.phoneNumber = phoneNumber;
        }

        public Letter()
        {
        }
    }
}
