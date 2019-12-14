namespace WSForm.Models
{
    public class LoginModel
    {
        public string username { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public string token { get; set; }

        public LoginModel()
        {
            username = "";
            name = "";
            role = "";
            token = "";
        }
    }
}