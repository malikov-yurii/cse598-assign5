<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e) 
    {
        // Load registered users from XML file
        string filePath = Server.MapPath("~/App_Data/registered-users.xml");
        Application["registeredUsers"] = LoadRegisteredUsers(filePath);
    }

    // Method to load registered users from XML file
    private Dictionary<string, string> LoadRegisteredUsers(string filePath)
    {
        var registeredUsers = new Dictionary<string, string>();

        if (System.IO.File.Exists(filePath))
        {
            var doc = new System.Xml.XmlDocument();
            doc.Load(filePath);

            foreach (System.Xml.XmlNode userNode in doc.SelectNodes("//user"))
            {
                string userId = userNode["userId"]?.InnerText;
                string password = userNode["password"]?.InnerText;

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(password))
                {
                    registeredUsers[userId] = password;
                }
            }
        }
        else
        {
            throw new Exception("registered-users.xml file not found in App_Data folder.");
        }

        return registeredUsers;
    }
</script>
