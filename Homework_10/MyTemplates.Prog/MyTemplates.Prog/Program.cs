using System;
using System.Collections.Generic;

namespace MyTemplates
{
    public class Students
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class Person
    {
        public string Fullname { get; set; }
        public string Subject { get; set; }
        public string Group { get; set; }
        public List<Students> Students { get; set; }
    }

    public class PersonModel
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var personModel = new PersonModel
            {
                FullName = "Иван Иванов",
                Age = 19,
                Address = "Улица Пушкина, дом Колотушкина"
            };
            var model = new Person()
            {
                Fullname = "Иван Иванов",
                Subject = "ОРИС",
                Group = "11-209",
                Students = new List<Students>
                {
                    new Students { Name = "Петр", Surname = "Петров" },
                    new Students { Name = "Анна", Surname = "Сидорова" },
                }
            };

            var farewellMessage = MyTemplates.GetFarewellMessage("Иван Иванов");
            var packageMessagePerson = MyTemplates.GetPackageMessage(personModel);
            var videoMessage = MyTemplates.GetVideoMessage(personModel);
            var groupMessage = MyTemplates.GetGroupMessage(model);

            Console.WriteLine(farewellMessage);
            Console.WriteLine(packageMessagePerson);
            Console.WriteLine(videoMessage);
            Console.WriteLine(groupMessage);
        }
    }
}