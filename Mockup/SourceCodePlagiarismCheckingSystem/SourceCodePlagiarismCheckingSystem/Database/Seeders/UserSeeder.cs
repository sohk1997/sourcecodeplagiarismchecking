using Library.Extension;
using SourceCodePlagiarismCheckingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SourceCodePlagiarismCheckingSystem.Database
{
    public class UserSeeder : EntitySeeder
    {
        private Random random = new Random();
        public override void Seed(AppDbContext appDbContext)
        {
            int count = appDbContext.Users.Count();
            DateTime startDate = new DateTime(1990, 1, 1);
            DateTime endDate = new DateTime(2008, 12, 31);
            int intervals = (endDate - startDate).Days;

            int[] genders = (int[])Enum.GetValues(typeof(Gender));
            for (int i = 0; i < (1000 - count); i++)
            {
                string firstname = firstNames[random.Next(firstNames.Length)];
                string lastname = lastNames[random.Next(lastNames.Length)];
                string randPic = profilePicture[random.Next(profilePicture.Length)];

                var countryId = appDbContext.Countries.Select(x => x.Id).Distinct().ToList();
                var randCountryId = countryId[random.Next(countryId.Count())];
                User user = new User()
                {
                    FirstName = firstname,
                    LastName = lastname,
                    DayOfBirth = startDate.AddDays(random.Next(intervals)),
                    Gender = (Gender)genders[random.Next(genders.Length)],
                    EmailAddress = GenerateEmailAddress($"{firstname}{lastname}"),
                    ProfilePicture = "layouts/layout/img/" + randPic ,
                    CountryId= randCountryId,
                    isActive=true
                };
                appDbContext.Users.Add(user);
            }
            appDbContext.SaveChanges();
        }
        private string GenerateEmailAddress(string emailName)
        {
            return string.Format("{0}{1}",
                 emailName.RemoveVietNameseCharacterMark().Replace(" ", string.Empty).ToLower(),
                 emailDomains[random.Next(emailDomains.Length)]);
        }

        #region data
        private string[] firstNames = {
            "Liam", "Emma", "Noah", "Olivia", "William", "Ava", "James", "Isabella", "Logan", "Sophia",
            "Benjamin", "Mia", "Mason", "Charlotte", "Elijah", "Amelia", "Oliver", "Evelyn", "Jacob",
            "Abigail", "Lucas", "Harper", "Michael", "Emily", "Alexander", "Elizabeth", "Ethan", "Avery",
            "Daniel", "Sofia", "Matthew", "Ella", "Aiden", "Madison", "Henry", "Scarlett", "Joseph",
            "Victoria", "Jackson", "Aria", "Samuel", "Grace", "Sebastian", "Chloe", "David", "Camila",
            "Carter", "Penelope", "Wyatt", "Riley", "Jayden", "Layla", "John", "Lillian", "Owen", "Nora",
            "Dylan", "Zoey", "Luke", "Mila", "Gabriel", "Aubrey", "Anthony", "Hannah", "Isaac", "Lily",
            "Grayson", "Addison", "Jack", "Eleanor", "Julian", "Natalie", "Levi", "Luna", "Christopher",
            "Savannah", "Joshua", "Brooklyn", "Andrew", "Leah", "Lincoln", "Zoe", "Mateo", "Stella",
            "Ryan", "Hazel", "Jaxon", "Ellie", "Nathan", "Paisley", "Aaron", "Audrey", "Isaiah", "Skylar",
            "Thomas", "Violet", "Charles", "Claire", "Caleb", "Bella", "Josiah", "Aurora", "Christian",
            "Lucy", "Hunter", "Anna", "Eli", "Samantha", "Jonathan", "Caroline", "Connor", "Genesis",
            "Landon", "Aaliyah", "Adrian", "Kennedy", "Asher", "Kinsley", "Cameron", "Allison", "Leo",
            "Maya", "Theodore", "Sarah", "Jeremiah", "Madelyn", "Hudson", "Adeline", "Robert", "Alexa",
            "Easton", "Ariana", "Nolan", "Elena", "Nicholas", "Gabriella", "Ezra", "Naomi", "Colton",
            "Alice", "Angel", "Sadie", "Brayden", "Hailey", "Jordan", "Eva", "Dominic", "Emilia", "Austin",
            "Autumn", "Ian", "Quinn", "Adam", "Nevaeh", "Elias", "Piper", "Jaxson", "Ruby", "Greyson",
            "Serenity", "Jose", "Willow", "Ezekiel", "Everly", "Carson", "Cora", "Evan", "Kaylee",
            "Maverick", "Lydia", "Bryson", "Aubree", "Jace", "Arianna", "Cooper", "Eliana", "Xavier",
            "Peyton", "Parker", "Melanie", "Roman", "Gianna", "Jason", "Isabelle", "Santiago", "Julia",
            "Chase", "Valentina", "Sawyer", "Nova", "Gavin", "Clara", "Leonardo", "Vivian", "Kayden",
            "Reagan", "Ayden", "Mackenzie", "Jameson", "Madeline"
        };

        private string[] lastNames = {
            "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore",
            "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia",
            "Martinez", "Robinson", "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen",
            "Young", "Hernandez", "King", "Wright", "Lopez", "Hill", "Scott", "Green", "Adams", "Baker",
            "Gonzalez", "Nelson", "Carter", "Mitchell", "Perez", "Roberts", "Turner", "Phillips",
            "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Sanchez", "Morris", "Rogers",
            "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey", "Rivera", "Cooper", "Richardson", "Cox",
            "Howard", "Ward", "Torres", "Peterson", "Gray", "Ramirez", "James", "Watson", "Brooks",
            "Kelly", "Sanders", "Price", "Bennett", "Wood", "Barnes", "Ross", "Henderson", "Coleman",
            "Jenkins", "Perry", "Powell", "Long", "Patterson", "Hughes", "Flores", "Washington", "Butler",
            "Simmons", "Foster", "Gonzales", "Bryant", "Alexander", "Russell", "Griffin", "Diaz", "Hayes"
        };

        private string[] emailDomains =
        {
            "@gmail.com", "@fpt.edu.vn"
        };

        private string[] profilePicture =
        {
            "avatar.png", "avatar1.jpg", "avatar2.jpg", "avatar3.jpg", "avatar4.jpg", "avatar5.jpg",
            "avatar6.jpg", "avatar7.jpg","avatar8.jpg", "avatar9.jpg", "avatar10.jpg","avatar11.jpg"
        };
        #endregion
    }
}