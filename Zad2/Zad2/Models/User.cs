using Zad2.Utils;

namespace Zad2.Models
{
    public class User
    {
		private string username;

		public string Username
		{
			get { return username; }
			set { username = value; }
		}

		private string  city;

		public string  City
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

		private string phoneNumber;

		public string PhoneNumber
		{
			get { return phoneNumber; }
			set { phoneNumber = value; }
		}

		private ConcurrentHashSet<string> blockedUsers;

        public ConcurrentHashSet<string> BlockedUsers
		{
			get { return blockedUsers; }
			set { blockedUsers = value; }
		}

		private bool isBusy;

		public bool IsBusy
		{
			get { return isBusy; }
			set { isBusy = value; }
		}

        public User(string username, string city, int age, string phoneNumber, bool isBusy)
        {
            this.username = username;
            this.city = city;
            this.age = age;
            this.phoneNumber = phoneNumber;
			this.isBusy = isBusy;
            this.blockedUsers = new ConcurrentHashSet<string>();
        }

        public User()
        {
            this.blockedUsers = new ConcurrentHashSet<string>();
        }
    }
}
