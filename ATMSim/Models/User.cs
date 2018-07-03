namespace ATMSim.Models
{
    public class User
    {
        public string User_Id { get; set; }
        public string Name { get; set; }

        public User()
        {

        }

        public User(string user_Id, string name)
        {
            User_Id = user_Id;
            Name = name;
        }
    }
}
