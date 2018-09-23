namespace StaffDirectory
{
    public class Employee
    {
        public int Id { get; }
        public string Name { get; }
        public string JobTitle { get; }
        public string EmailAddress { get; }
        public int? ManagerId { get; }

        public Employee(int id, string name, string jobTitle, string emailAddress, int? managerId)
        {
            Id = id;
            Name = name;
            JobTitle = jobTitle;
            EmailAddress = emailAddress;
            ManagerId = managerId;
        }

        public override bool Equals(object obj)
        {
            var result = false;
            if (obj != null && obj.GetType() == typeof(Employee))
            {
                var that = (Employee)obj;
                result =
                    Id.Equals(that.Id) &&
                    Name.Equals(that.Name) &&
                    JobTitle.Equals(that.JobTitle) &&
                    EmailAddress.Equals(that.EmailAddress) &&
                    ManagerId.Equals(that.ManagerId);
            }
            return result;
        }

        public override string ToString()
        {
            return
$@"Id: {Id}
Name: {Name}
Job Title: {JobTitle}
Email Address: {EmailAddress}
Manager Id: {ManagerId}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (JobTitle != null ? JobTitle.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (EmailAddress != null ? EmailAddress.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ManagerId.GetHashCode();
                return hashCode;
            }
        }
    }
}