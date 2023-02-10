using dotenv.net;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

var env = DotEnv.Read();

MongoClient dbClient = new MongoClient(env["MONGODB"]);

var database = dbClient.GetDatabase (env["MONGO_DATABASE"]);
var students = database.GetCollection<BsonDocument> (env["MONGO_STUDENTS"]);
var workers = database.GetCollection<BsonDocument> (env["MONGO_WORKERS"]);
var classes = database.GetCollection<BsonDocument> (env["MONGO_CLASSES"]);
var applications = database.GetCollection<BsonDocument> (env["MONGO_APPLICATIONS"]);
var questionnaire = database.GetCollection<BsonDocument> (env["MONGO_QUESTIONNAIRE"]);

bool isLoged = false;
bool isStudent = true;
var password = string.Empty;
var login = string.Empty;

while (true)
{
    while (isLoged != true)
    {
        Console.WriteLine();
        isLoged = false;
        password = string.Empty;

        // Login on console
        Console.Write("Login: ");
        login = Console.ReadLine();

        // Password login on console with limit of 25 characters
        Console.Write("Password: ");
        ConsoleKey key;

        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar) && !(password.Length >= 25) )
            {
                Console.Write("*");
                password += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        // Do a line of space
        Console.WriteLine();
        Console.WriteLine();

        // Search for the user in the database
        var filter = Builders<BsonDocument>.Filter.Eq("login", login);
        
        if (login=="admin" && password=="haslo123")
        {
            Console.WriteLine("Logowanie Udane!");
            isLoged = true;
            isStudent = false;
        }
        // Get the user
        else if (students.Find(filter).FirstOrDefault() != null)
        {
            var user = students.Find(filter).FirstOrDefault();
            // Check if the password is correct
            if ( password == user["password"])
            {
                Console.WriteLine("Logowanie Udane!");
                isLoged = true;
            }
            else
            {
                Console.WriteLine("Logowanie Nieudane!");
            }
        }
        else if (workers.Find(filter).FirstOrDefault() != null)
        {
            var user = workers.Find(filter).FirstOrDefault();
            // Check if the password is correct
            if (password == user["password"])
            {
                Console.WriteLine("Logowanie Udane!");
                isLoged = true;
                isStudent = false;
            }
            else
            {
                Console.WriteLine("Logowanie Nieudane!");
            }
        }
        else
        {
            Console.WriteLine("Logowanie Nieudane!");
        }

        // Do a line of space
        Console.WriteLine();
    };

    // Menu for student and diffrent menu for worker
    if (isStudent)
    {
        Console.WriteLine("Menu Studenta: ");
        Console.WriteLine("1. Zobacz oceny");
        Console.WriteLine("2. Zobacz twoje dane");
        Console.WriteLine("3. Złóż wniosek");
        Console.WriteLine("4. Ankiety");
        Console.WriteLine("5. Zmień hasło");
        Console.WriteLine("6. Wyloguj");
        Console.WriteLine();
        Console.Write("Wybierz opcje: ");
        string option = Console.ReadLine();
        Console.WriteLine();
        
        //get the student
        var filter = Builders<BsonDocument>.Filter.Eq("login", login);
        var user = students.Find(filter).FirstOrDefault();

        switch (option)
        {
            case "1":
                Console.WriteLine("Oceny");
                // displays the grades of the student
                // in array
                var grades = user["grades"];
                foreach (var grade in grades.AsBsonArray)
                {
                    // Fancy way to print the grades
                    Console.WriteLine(grade["subject"] + " " + grade["value"]);
                }
                Console.WriteLine();
                break;
            case "2":
                Console.WriteLine("Twoje dane: ");
                Console.WriteLine();
                var studentName = user["name"];
                var studentFirstName = studentName["firstName"].ToString();
                var studentLastName = studentName["lastName"].ToString();
                var studentMiddleName = studentName["midName"].ToString();
                var studentBirthDate = user["dateOfBirth"].ToString();
                var studentIndex = user["index"].ToString();

                var studentAddress = user["address"];
                var studentStreet = studentAddress["street"].ToString();
                var studentCity = studentAddress["city"].ToString();
                var studentPostalCode = studentAddress["postalCode"].ToString();
                var studentCountry = studentAddress["Country"].ToString();
                var studentHouse = studentAddress["HouseNumber"].ToString();
                var studentApartament = studentAddress["ApartmentNumber"].ToString();
                var studentPhone = user["phoneNumber"].ToString();
                var studentEmail = user["email"].ToString();

                var studentClass = user["class"];
                var studentClassId = studentClass["id"].ToString();
                var studentClassLab = studentClass["lab"].ToString();
                var studentClassCwi = studentClass["cwi"].ToString();
                var studentClassAng = studentClass["ang"].ToString();
                

                // Print the student data
                Console.WriteLine("Imie: " + studentFirstName);
                Console.WriteLine("Nazwisko: " + studentLastName);
                Console.WriteLine("Drugie imie: " + studentMiddleName);
                Console.WriteLine("Data urodzenia: " + studentBirthDate);
                Console.WriteLine("Numer indeksu: " + studentIndex);
                Console.WriteLine("Ulica: " + studentStreet);
                Console.WriteLine("Miasto: " + studentCity);
                Console.WriteLine("Kod pocztowy: " + studentPostalCode);
                Console.WriteLine("Kraj: " + studentCountry);
                Console.WriteLine("Numer domu: " + studentHouse);
                Console.WriteLine("Numer mieszkania: " + studentApartament);
                Console.WriteLine("Numer telefonu: " + studentPhone);
                Console.WriteLine("Email: " + studentEmail);
                Console.WriteLine("Klasa: " + studentClassId);
                Console.WriteLine("Lab: " + studentClassLab);
                Console.WriteLine("Cwi: " + studentClassCwi);
                Console.WriteLine("Ang: " + studentClassAng);
                Console.WriteLine();

                break;
                case "3":
                Console.WriteLine("Rodzaje wniosków");
                
                Console.WriteLine("1. Podanie o przedłużenie możliwości zaliczenia kursu");
                Console.WriteLine("2. Podanie - odwołanie od decyzji o skreśleniu z listy studentów");
                Console.WriteLine("3. Podanie o warunkowy wpis na semestr");
                Console.WriteLine("4. Podanie o przeniesienie z innej uczelni");
                Console.WriteLine("5. Podanie o przepisanie oceny");
                int opt=7;
                string topic="0";
                // var studentInde = user["index"].ToString();
                // var wniosekFilter = Builders<BsonDocument>.Filter.Eq("index", studentInde);
                while(opt<0||opt>5){
                
                opt=int.Parse(Console.ReadLine());
                if(opt<0||opt>6)Console.WriteLine("Nieprawidłowy numer wniosku");}
                switch(opt){
                 case 1:
                  topic="Podanie o przedłużenie możliwości zaliczenia kursu";
                  break;
                 case 2:
                  topic="2. Podanie - odwołanie od decyzji o skreśleniu z listy studentów";
                  break;
                 case 3:
                  topic="3. Podanie o warunkowy wpis na semestr";
                  break;
                 case 4:
                  topic="4. Podanie o przeniesienie z innej uczelni";
                  break;
                 case 5:
                  topic="5. Podanie o przepisanie oceny";
                  break;
                 default:
                  break;
                }
                Console.WriteLine(topic);
                
                // Create a new document for the application and insert it into the database
                var wniosek = new BsonDocument
                {
                    { "index", user["index"].ToString() },
                    { "topic", topic },
                    { "status", "nierozpatrzony" },
                    { "date", DateTime.Now.ToString("dd/MM/yyyy") }
                };

                applications.InsertOne(wniosek);

                break;
            case "4":
                Console.WriteLine("Wypełnij ankietę");

                // Check if any questionnaire is waiting for approval
                var questFilter = Builders<BsonDocument>.Filter.Eq("odp1", "---");
                var quest = questionnaire.Find(questFilter).FirstOrDefault();

                // If there is no questionnaire waiting for approval
                if (quest == null)
                {
                    Console.WriteLine("Brak wnioskow do rozpatrzenia");
                    Console.WriteLine();
                    break;
                }
                else
                {
                Console.WriteLine("Wpisz numer indeksu");
                int ankindex=int.Parse(Console.ReadLine());

                var czyFilter = Builders<BsonDocument>.Filter.Eq("numer indeksu", ankindex);
                var czy = questionnaire.Find(czyFilter).FirstOrDefault();
               
                if(czy==null){

                var pyt1 = quest["pytanie1"];
                Console.WriteLine(pyt1);
                Console.WriteLine("Wpisz odpowiedż");
                string odp1=Console.ReadLine();

                var pyt2 = quest["pytanie2"];
                Console.WriteLine(pyt2);
                Console.WriteLine("Wpisz odpowiedź");
                string odp2=Console.ReadLine();

                var pyt3 = quest["pytanie3"];
                Console.WriteLine(pyt3);
                Console.WriteLine("Wpisz odpowiedź");
                string odp3=Console.ReadLine();

                var pyt4 = quest["pytanie4"];
                Console.WriteLine(pyt4);
                Console.WriteLine("Wpisz odpowiedź");
                string odp4=Console.ReadLine();

                
                Console.WriteLine("Wpisz uwagi");
                string uwagi=Console.ReadLine();


                 var ankieta = new BsonDocument
                {
                    { "numer indeksu", ankindex },
                    { "pytanie1", pyt1 },
                    { "odp1", odp1 },
                    { "pytanie2", pyt2 },
                    { "odp2", odp2 },
                    { "pytanie3", pyt3 },
                    { "odp3", odp3 },
                    { "pytanie4", pyt4 },
                    { "odp4", odp4 },
                    { "uwagi", uwagi }
                };
                questionnaire.InsertOne(ankieta);}
                else Console.WriteLine("wypełniłeś już tę ankietę");






                Console.WriteLine();
                }



                
                break;
            case "5":
                Console.WriteLine("Zmień hasło");
                bool isPasswordNotChanged = true;

                while(isPasswordNotChanged) {
                    // Get the new password
                    Console.Write("Podaj nowe hasło: ");
                    string newPassword = Console.ReadLine();
                    Console.Write("Potwierdź hasło: ");
                    string newPasswordConfirm = Console.ReadLine();
                    // Check if the passwords are the same
                    if (newPassword == newPasswordConfirm)
                    {
                        // Update the password
                        var update = Builders<BsonDocument>.Update.Set("password", newPassword);
                        students.UpdateOne(filter, update);
                        Console.WriteLine("Hasło zmienione!");
                        Console.WriteLine();
                        isPasswordNotChanged = false;
                    }
                    else
                    {
                        Console.WriteLine("Hasła nie są takie same!");
                        Console.WriteLine();
                    }
                }
                break;
            case "6":
                Console.WriteLine("Wyloguj");
                isLoged = false;
                break;
            default:
                Console.WriteLine("Nie ma takiej opcji");
                break;
        }

    }
    else
    {
        Console.WriteLine("Menu Pracownika: ");
        Console.WriteLine("1. Dodaj studenta");
        Console.WriteLine("2. Dodaj nauczyciela");
        Console.WriteLine("3. Zarządzanie grupami");
        Console.WriteLine("4. Dodaj ankietę");
        Console.WriteLine("5. Dodaj ocene");
        Console.WriteLine("6. Zmien hasło");
        Console.WriteLine("7. rozpatrz wnioski");
        Console.WriteLine("8. Wyloguj");
        Console.WriteLine();
        Console.Write("Wybierz opcje: ");
        string option = Console.ReadLine();
        Console.WriteLine();

        // Get the worker
        var filter = Builders<BsonDocument>.Filter.Eq("login", login);
        var user = workers.Find(filter).FirstOrDefault();

        switch (option)
        {
            case "1":
                Console.WriteLine("Dodaj studenta");
                Console.WriteLine("imię studenta");
                var firstName = Console.ReadLine();
                Console.WriteLine("drugie imię studenta");
                var midName = Console.ReadLine();
                Console.WriteLine("nazwisko studenta");
                var lastName = Console.ReadLine();
                Console.WriteLine("numer indeksu studenta");
                int index = int.Parse(Console.ReadLine());
                Console.WriteLine("data urodzenia studenta");
                string data = Console.ReadLine();
                //checking date format
                int d=0,m=0,y=0;
                if(data.Length==10||data.Length==8){
                    if(data[2]=='-'){
                                String[] list = data.Split("-");
                                int[] da = Array.ConvertAll(data.Split("-"), new Converter<string, int>(int.Parse));
                                d=da[0];m=da[1];y=da[2];
                    }
                    else{
                        String[] list = data.Split(".");
                                int[] da = Array.ConvertAll(data.Split("."), new Converter<string, int>(int.Parse));
                                d=da[0];m=da[1];y=da[2];  
                    }
                }
                else if(data.Length==9||data.Length==7){
                    if(data[1]=='-'){
                                String[] list = data.Split("-");
                                int[] da = Array.ConvertAll(data.Split("-"), new Converter<string, int>(int.Parse));
                                d=da[0];m=da[1];y=da[2];
                    }
                    else{
                        String[] list = data.Split(".");
                                int[] da = Array.ConvertAll(data.Split("."), new Converter<string, int>(int.Parse));
                                d=da[0];m=da[1];y=da[2];      
                    }
                }
                Console.WriteLine("hasło studenta");
                // read password and hash it usimng bcrypt
                var haslo = Console.ReadLine();
                
                Console.WriteLine("mail studenta");
                //checking if mail is correct

                bool pop=false;
                string mail=string.Empty;
                while(pop==false){
                mail = Console.ReadLine();
                String[] e = mail.Split("@");
                                
                if (e.Length==1) {
                    Console.WriteLine("mail jest niepoprawny, podaj poprawny mail");
                }
                else{
                    String[] em =e[1].Split(".");
                    if (em.Length==1) {
                    Console.WriteLine("mail jest niepoprawny, podaj poprawny mail");}
                    else{
                    Console.WriteLine("mail jest poprawny");
                    pop=true;}}
                }
                Console.WriteLine("numer kontaktowy studenta");
                var phonenumber = Console.ReadLine();
                Console.WriteLine("podaj adres studenta");
                Console.WriteLine("kraj:");
                var country= Console.ReadLine();
                Console.WriteLine("miasto:");
                var city= Console.ReadLine();
                Console.WriteLine("ulica:");
                var street= Console.ReadLine();
                Console.WriteLine("nr domu:");
                var HouseNumber= Console.ReadLine();
                Console.WriteLine("nr mieszkania:");
                var ApartmentNumber= Console.ReadLine();
                Console.WriteLine("kod pocztowy:");
                var postalCode= Console.ReadLine();
                




                var student = new BsonDocument
    {
        { "name", new BsonDocument
            {
                { "firstName", firstName },
                { "midName", midName },
                { "lastName", lastName }
            } 
        },
        { "index", index },
        { "dateOfBirth", new DateTime(y, m, d+1) },
        { "class", new BsonDocument
            {
                { "id", ObjectId.Parse("6374d25b215d99eb272d1f14") },
                { "specialization", "Def" },
                { "lab", 0 },
                { "cwi", 0 },
                { "ang", 0 }
            }
        },
        { "login", index.ToString() },
        { "password", haslo },
        { "email", mail },
        { "phoneNumber", phonenumber },
        { "address", new BsonDocument
            {
                { "Country", country },
                { "HouseNumber", HouseNumber },
                { "ApartmentNumber", ApartmentNumber },
                { "street", street },
                { "city", city },
                { "postalCode", postalCode }
            }
        },
        { "specialization", "" },
        // Grades array
        { "grades", new BsonArray() }
    };

                students.InsertOne(student);

                break;
            case "2":
                Console.WriteLine("Dodaj nauczyciela");

                // Get teacher name
                Console.WriteLine("Podaj imie nauczyciela");
                string teacherFirstName = Console.ReadLine();
                Console.WriteLine("Podaj drugie imie nauczyciela");
                string teacherMidName = Console.ReadLine();
                Console.WriteLine("Podaj nazwisko nauczyciela");
                string teacherLastName = Console.ReadLine();

                // Get teacher date of birth
                Console.WriteLine("Podaj date urodzenia nauczyciela");
                string teacherDateOfBirth = Console.ReadLine();

                // Get teacher login
                Console.WriteLine("Podaj login nauczyciela");
                string teacherLogin = Console.ReadLine();

                // Get teacher password
                Console.WriteLine("Podaj haslo nauczyciela");
                string teacherPassword = Console.ReadLine();

                // Get teacher email
                Console.WriteLine("Podaj email nauczyciela");
                string teacherEmail = Console.ReadLine();

                // Get teacher phone number
                Console.WriteLine("Podaj numer telefonu nauczyciela");
                string teacherPhoneNumber = Console.ReadLine();

                // Get teacher salary
                Console.WriteLine("Podaj pensje nauczyciela");
                string teacherSalary = Console.ReadLine();

                // Get teacher position
                Console.WriteLine("Podaj stanowisko nauczyciela");
                string teacherPosition = Console.ReadLine();

                // Get teacher subjects
                Console.WriteLine("Podaj przedmioty nauczyciela");
                string teacherSubjects = Console.ReadLine();

                // Get teacher degree
                Console.WriteLine("Podaj stopien nauczyciela");
                string teacherDegree = Console.ReadLine();

                // Get teacher country
                Console.WriteLine("Podaj kraj nauczyciela");
                string teacherCountry = Console.ReadLine();

                // Get teacher city
                Console.WriteLine("Podaj miasto nauczyciela");
                string teacherCity = Console.ReadLine();

                // Get teacher street
                Console.WriteLine("Podaj ulice nauczyciela");
                string teacherStreet = Console.ReadLine();

                // Get teacher house number
                Console.WriteLine("Podaj numer domu nauczyciela");
                string teacherHouseNumber = Console.ReadLine();

                // Get teacher apartment number
                Console.WriteLine("Podaj numer mieszkania nauczyciela");
                string teacherApartmentNumber = Console.ReadLine();

                // Get teacher postal code
                Console.WriteLine("Podaj kod pocztowy nauczyciela");
                string teacherPostalCode = Console.ReadLine();


                var worker = new BsonDocument
                {
                    { "name", new BsonDocument
                        {
                            { "firstName", teacherFirstName },
                            { "midName", teacherMidName },
                            { "lastName", teacherLastName }
                        } 
                    },
                    { "dateOfBirth", teacherDateOfBirth },
                    { "login", teacherLogin },
                    { "password", teacherPassword },
                    { "email", teacherEmail },
                    { "phoneNumber", teacherPhoneNumber },
                    { "salary", teacherSalary },
                    { "position", teacherPosition },
                    { "subjects", teacherSubjects },
                    { "degree", teacherDegree },
                    { "address", new BsonDocument
                        {
                            { "Country", teacherCountry },
                            { "HouseNumber", teacherHouseNumber },
                            { "ApartmentNumber", teacherApartmentNumber },
                            { "street", teacherStreet },
                            { "city", teacherCity },
                            { "postalCode", teacherPostalCode }
                        }
                    },
                };

                workers.InsertOne(worker);

                break;
            case "3":
                Console.WriteLine("Wpisz nr indeksu studenta");
                string groupindex=Console.ReadLine();
                Console.WriteLine("1. Zmień specjalizację studenta");
                Console.WriteLine("2. Zmień grupę labolatoryjną studenta");
                Console.WriteLine("3. Zmień grupę ćwiczeniową studenta");
                Console.WriteLine("4. Zmień grupę językową studenta");
                int opt=int.Parse(Console.ReadLine());

                
                var specIndexFilter = Builders<BsonDocument>.Filter.Eq("index", int.Parse(groupindex));
                // Change value of specialization field in class in student document

                switch(opt){
                    case 1:
                        Console.WriteLine("Podaj nową specjalizację");
                        var spec=Console.ReadLine();
                        var specUpdate = Builders<BsonDocument>.Update.Set("class.specialization", spec);
                        students.UpdateOne(specIndexFilter, specUpdate);
                    break;
                    case 2:
                        Console.WriteLine("Podaj nową grupę labolatoryjną");
                        var lab=Console.ReadLine();
                        var labUpdate = Builders<BsonDocument>.Update.Set("class.lab", lab);
                        students.UpdateOne(specIndexFilter, labUpdate);                   
                    break;
                    case 3:
                        Console.WriteLine("Podaj nową grupę ćwiczeniową");
                        var cw=Console.ReadLine();
                        var cwUpdate = Builders<BsonDocument>.Update.Set("class.cwi", cw);
                        students.UpdateOne(specIndexFilter, cwUpdate);                    
                    break;
                    case 4:
                        Console.WriteLine("Podaj nową grupę językową");
                        var lang=Console.ReadLine();
                        var langUpdate = Builders<BsonDocument>.Update.Set("class.ang", lang);
                        students.UpdateOne(specIndexFilter, langUpdate);                   
                    break;

                }


                break;
            case "4":
                Console.WriteLine("Dodaj ankietę");
                // Console.WriteLine("Wpisz nr ankiety");
                // int nrankiety=int.Parse(Console.ReadLine());
                Console.WriteLine("Wpisz pytanie nr 1");
                string pyt1=Console.ReadLine();
                Console.WriteLine("Wpisz pytanie nr 2");
                string pyt2=Console.ReadLine();
                Console.WriteLine("Wpisz pytanie nr 3");
                string pyt3=Console.ReadLine();
                Console.WriteLine("Wpisz pytanie nr 4");
                string pyt4=Console.ReadLine();

                 var ankieta = new BsonDocument
                {
                    // { "numer ankiety", nrankiety },
                    { "pytanie1", pyt1 },
                    { "odp1", "---" },
                    { "pytanie2", pyt2 },
                    { "odp2", "---" },
                    { "pytanie3", pyt3 },
                    { "odp3", "---" },
                    { "pytanie4", pyt4 },
                    { "odp4", "---" },
                    { "uwagi", "---" }
                };
                questionnaire.InsertOne(ankieta);
                break;
            case "5":


                Console.WriteLine("Dodaj ocene");

                // looking for student by index
                Console.WriteLine("Podaj numer indeksu studenta");
                int ocenaIndex = int.Parse(Console.ReadLine());
                var ocenaFilter = Builders<BsonDocument>.Filter.Eq("index", ocenaIndex);
                var ocenaStudent = students.Find(ocenaFilter).FirstOrDefault();

                // Get stubject name 
                Console.WriteLine("Podaj nazwę przedmiotu");
                string ocenaSubject = Console.ReadLine();

                // Get grade value
                Console.WriteLine("Podaj ocene");
                int ocenaValue = int.Parse(Console.ReadLine());

                // Insert grade to grades object in student object
                var ocenaUpdate = Builders<BsonDocument>.Update.Push("grades", new BsonDocument
                {
                    { "subject", ocenaSubject },
                    { "value", ocenaValue }
                });
                students.UpdateOne(ocenaFilter, ocenaUpdate);

                break;
            case "6":
                Console.WriteLine("Zmień hasło");
                // Change worker password
                bool isPasswordNotChanged = true;

                while(isPasswordNotChanged) {
                    // Get the new password
                    Console.Write("Podaj nowe hasło: ");
                    string newPassword = Console.ReadLine();
                    Console.Write("Potwierdź hasło: ");
                    string newPasswordConfirm = Console.ReadLine();
                    // Check if the passwords are the same
                    if (newPassword == newPasswordConfirm)
                    {
                        // Update the password
                        var passwordUpdate = Builders<BsonDocument>.Update.Set("password", newPassword);
                        workers.UpdateOne(filter, passwordUpdate);
                        Console.WriteLine("Hasło zmienione!");
                        Console.WriteLine();
                        isPasswordNotChanged = false;
                    }
                    else
                    {
                        Console.WriteLine("Hasła nie są takie same!");
                        Console.WriteLine();
                    }
                }
                break;
            case "7":
                Console.WriteLine("Rozpatrz wniosek");

                var test = applications.Find(_ => true).ToList();
                var application = test.Where(x => x["status"] == "nierozpatrzony").FirstOrDefault();
                // Check if any application is waiting for approval
                //var applicationFilter = Builders<BsonDocument>.Filter.Eq("status", "oczekuje");
                //var application = applications.Find(applicationFilter).FirstOrDefault();

                // If there is no application waiting for approval
                if (application == null)
                {
                    Console.WriteLine("Brak wniosków do rozpatrzenia");
                    Console.WriteLine();
                    break;
                }
                else
                {
                var topic = application["topic"];
                Console.WriteLine(topic);
                Console.WriteLine("pozytywny czy negatywny");
                string rozpatrz=Console.ReadLine();

                var applicationupdate = Builders<BsonDocument>.Update.Set("status", rozpatrz);
                applications.UpdateOne(application, applicationupdate);
                Console.WriteLine();
                }
                break;
            case "8":
                Console.WriteLine("Wyloguj");
                isLoged = false;
                isStudent = true;
                break;
            default:
                Console.WriteLine("Nie ma takiej opcji");
                break;
        }
    }
}