namespace gogobuy.Models
{
    public class CMembership
    {
        public int fMemberID { get; set; }
        public string fFirstName { get; set; }
        public string fLastName { get; set; }
        public string fAddress { get; set; }
        public string fEmail { get; set; }
        public string fPhone { get; set; }
        public string fDateOfBirth { get; set; }
        public bool fGender { get; set; }
        public bool fEmailVerified { get; set; }
        public string fPassword { get; set; }
        public string fSalt { get; set; }

    }
}