using KA_MSSQL_Procvicovani;
using Microsoft.Data.SqlClient;

string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\EDM_PC\Documents\Programy\VS\KA_MSSQL_Procvicovani\KA_MSSQL_Procvicovani\KA_MSSQL_Procvicovani\hry.mdf"";Integrated Security=True;Connect Timeout=30";

while (true)
{
    char input;
    Console.Clear();
    Console.WriteLine("1. Insert");
    Console.WriteLine("2. Select");
    Console.WriteLine("3. Update");
    Console.WriteLine("4. Select + Sort");
    Console.WriteLine("5. Delete");
    Console.WriteLine("6. Select + Agregační funkce");
    Console.WriteLine("7. Exit");

    Console.WriteLine();
    Console.Write("Vyberte co chcete dělat: ");
    input = Console.ReadKey().KeyChar;
    Console.WriteLine();
    Console.Clear();

    switch (input)
    {
        case '1':
            Insert();
            break;
        case '2':
            Select();
            break;
        case '3':
            Update();
            break;
        case '4':
            Sort();
            break;
        case '5':
            Delete();
            break;
        case '6':
            Aggregate();
            break;
        case '7':
            Environment.Exit(0);
            break;
    }  
} 
void Insert() 
{
    SqlConnection connection = new SqlConnection(ConnectionString);
    Console.WriteLine("Tabulky:\n1. Games\n2. Game Studio");
    string query = null;
    SqlCommand cmd;
    char input;
    do
    {
        Console.Write("Vyberte tabulku na INSERT (1, 2)");
        input = Console.ReadKey().KeyChar;
        Console.Clear();
        if (input == '1')
        {
            Games game = new Games();
            Console.Write("Název hry: ");
            game.Title = Console.ReadLine();
            Console.Write("Žánr hry: ");
            game.Genre = Console.ReadLine();
            Console.Write("Kolik stojí (Kč): ");
            game.Price = double.Parse(Console.ReadLine());
            Console.Write("Hodnocení hry (1-10): ");
            game.Rating = int.Parse(Console.ReadLine());
            Console.WriteLine();

            connection.Open();
            cmd = new SqlCommand("select Id, Name from games", connection);
            SqlDataReader read = cmd.ExecuteReader();
            Console.WriteLine("Existující studia:");
            while (read.Read())
            {
                Console.WriteLine($"{read["Id"]}. {read["Name"]}");
            }
            read.Close();

            Console.Write("ID studia: ");
            game.Id_Studio = int.Parse(Console.ReadLine());

            query = "insert into Games (Title, Genre, Price, Rating, Id_Studio) values (@Title, @Genre, @Price, @Rating, @Id_Studio)";
            cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Title", game.Title);
            cmd.Parameters.AddWithValue("@Genre", game.Genre);
            cmd.Parameters.AddWithValue("@Price", game.Price);
            cmd.Parameters.AddWithValue("@Rating", game.Rating);
            cmd.Parameters.AddWithValue("@Id_Studio", game.Id_Studio);
            cmd.ExecuteNonQuery();
            Vypis(new SqlCommand("select * from gamestudio", connection));
            connection.Close();
        }
        else if (input == '2')
        {
            Studio studio = new Studio();
            Console.Write("Napište název studia: ");
            studio.Name = Console.ReadLine();
            Console.Write("Kde se nachází studio: ");
            studio.Location = Console.ReadLine();
            Console.Write("Kolik je zaměstnanců: ");
            studio.Employees = int.Parse(Console.ReadLine());
            Console.Write("Reputace: ");
            studio.Reputation = Console.ReadLine();

            connection.Open();
            query = "insert into GameStudio (Name, Location, Employees, Reputation) values (@Name, @Location, @Employees, @Reputation)";
            cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", studio.Name);
            cmd.Parameters.AddWithValue("@Location", studio.Location);
            cmd.Parameters.AddWithValue("@Employees", studio.Employees);
            cmd.Parameters.AddWithValue("@Reputation", studio.Reputation);
            cmd.ExecuteNonQuery();
            Vypis(new SqlCommand("select * from games", connection));
            connection.Close();
        }
        else
        {
            Console.WriteLine("\nTabulka neexistuje!");
        }
        Console.ReadLine();
    } 
    while (input != '1' && input != '2');
}
void Select() 
{
    SqlConnection connection = new SqlConnection(ConnectionString);
    Console.WriteLine("Tabulky:\n1. Games\n2. Game Studio");
    string query = null;
    SqlCommand cmd;
    while (true)
    {
        Console.WriteLine("Napiště váš dotaz: ");
        query = Console.ReadLine().ToLower();
        if (query.Contains("select"))
        {
            cmd = new SqlCommand(query, connection);
            break;
        }
        else 
            Console.WriteLine("Tento dotaz neobsahuje SELECT!");
    }
    connection.Open();
    Vypis(cmd);
    connection.Close();

    Console.ReadLine();
}
void Update()
{
    SqlConnection connection = new SqlConnection(ConnectionString);
    Console.WriteLine("Tabulky:\n1. Games\n2. Game Studio");
    string query = null;
    SqlCommand cmd;
    char input;
    do
    {
        Console.Write("Vyberte tabulku na UPDATE (1, 2): ");
        input = Console.ReadKey().KeyChar;

        if (input == '1')
        {
            connection.Open();
            cmd = new SqlCommand("select * from games", connection);
            Vypis(cmd);
            connection.Close();
            Console.WriteLine("");
            Console.Write("Podle ID vyberte sloupec na UPDATE: ");
            char id = Console.ReadKey().KeyChar;
            Console.WriteLine();

            Games game = new Games();
            Console.Write("Název hry: ");
            game.Title = Console.ReadLine();
            Console.Write("Žánr hry: ");
            game.Genre = Console.ReadLine();
            Console.Write("Kolik stojí (Kč): ");
            game.Price = double.Parse(Console.ReadLine());
            Console.Write("Hodnocení hry (1-10): ");
            game.Rating = int.Parse(Console.ReadLine());
            Console.Write("OPRAVDU CHCETE POTVRDIT ZMĚNU? (Y/N)");

            if (Console.ReadLine().ToLower() != "y") break;

            query = "update Games set Title = @Title, Genre = @Genre, Price = @Price, Rating = @Rating where Id = @Id";
            connection.Open();
            cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Title", game.Title);
            cmd.Parameters.AddWithValue("@Genre", game.Genre);
            cmd.Parameters.AddWithValue("@Price", game.Price);
            cmd.Parameters.AddWithValue("@Rating", game.Rating);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            Vypis(new SqlCommand("select * from games", connection));
            connection.Close();

        }
        else if (input == '2')
        {
            connection.Open();
            cmd = new SqlCommand("select * from gamestudio", connection);
            Vypis(cmd);
            connection.Close();
            Console.WriteLine("");
            Console.Write("Podle ID vyberte sloupec na UPDATE: ");
            char id = Console.ReadKey().KeyChar;
            Console.WriteLine();

            Studio studio = new Studio();
            Console.Write("Napište název studia: ");
            studio.Name = Console.ReadLine();
            Console.Write("Kde se nachází studio: ");
            studio.Location = Console.ReadLine();
            Console.Write("Kolik je zaměstnanců: ");
            studio.Employees = int.Parse(Console.ReadLine());
            Console.Write("Reputace: ");
            studio.Reputation = Console.ReadLine();
            Console.Write("OPRAVDU CHCETE POTVRDIT ZMĚNU? (Y/N) ");

            if (Console.ReadLine().ToLower() != "y") break;

            query = "update GameStudio set Name = @Name, Location = @Location, Employees = @Employees, Reputation = @Reputation where Id = @Id";
            connection.Open();
            cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", studio.Name);
            cmd.Parameters.AddWithValue("@Location", studio.Location);
            cmd.Parameters.AddWithValue("@Employees", studio.Employees);
            cmd.Parameters.AddWithValue("@Reputation", studio.Reputation);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            Vypis(new SqlCommand("select * from gamestudio", connection));
            connection.Close();
        }
        else
            Console.WriteLine("\nTabulka neexistuje!");
        Console.ReadLine();
    }
    while (input != '1' && input != '2');
}
void Sort() 
{
    SqlConnection connection = new SqlConnection(ConnectionString);
    Console.WriteLine("Tabulky:\n1. Games\n2. Game Studio");
    string query = null;
    SqlCommand cmd;
    char input;
    do
    {
        Console.Write("Vyberte tabulku na SORT (1, 2): ");
        input = Console.ReadKey().KeyChar;
        if (input == '1')
        {
            connection.Open();
            Vypis(new SqlCommand("select * from games", connection));
            connection.Close();
            Console.Write("Sloupec podle kterého chcete seřadit: ");
            string column = Console.ReadLine();
            Console.Write("Jak chcete seřadit? (ASC/DESC): ");
            string order = Console.ReadLine();
            connection.Open();
            Vypis(new SqlCommand($"select * from games order by {column} {order}",connection));
            connection.Close();
        }
        else if (input == '2')
        {
            connection.Open();
            Vypis(new SqlCommand("select * from gamestudio", connection));
            connection.Close();
            Console.Write("Sloupec podle kterého chcete seřadit: ");
            string column = Console.ReadLine();
            Console.Write("Jak chcete seřadit? (ASC/DESC): ");
            string order = Console.ReadLine().ToLower();
            connection.Open();
            Vypis(new SqlCommand($"select * from gamestudio order by {column} {order}", connection));
            connection.Close();
        }
        else
            Console.WriteLine("\nTabulka neexistuje!");
        Console.ReadLine();
    }
    while (input != '1' && input != '2');
}
void Delete()
{
    SqlConnection connection = new SqlConnection(ConnectionString);
    Console.WriteLine("Tabulky:\n1. Games\n2. Game Studio");
    string query = null;
    SqlCommand cmd;
    char input;
    do
    {
        Console.Write("Vyberte tabulku na DELETE (1, 2): ");
        input = Console.ReadKey().KeyChar;
        if (input == '1')
        {
            connection.Open();
            Vypis(new SqlCommand("select * from games", connection));
            connection.Close();
            Console.Write("ID řádku který chcete smazat: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("OPRAVDU CHCETE SMAZAT ZÁZNAM? (Y/N) ");
            connection.Open();

            if (Console.ReadLine().ToLower() == "y")
            {
                cmd = new SqlCommand("delete from Games where Id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                Vypis(new SqlCommand("select * from games", connection));
            }
            else break;
            connection.Close();
        }
        if (input == '2')
        {
            connection.Open();
            Vypis(new SqlCommand("select * from gamestudio", connection));
            connection.Close();
            Console.Write("ID řádku který chcete smazat: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("OPRAVDU CHCETE SMAZAT ZÁZNAM? (Y/N) ");
            if (Console.ReadLine().ToLower() == "y")
            {
                connection.Open();
                cmd = new SqlCommand("delete from gamestudio where Id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                Vypis(new SqlCommand("select * from gamestudio", connection));
                connection.Close();
            }
            else break;
        }
        else
            Console.WriteLine("\nTabulka neexistuje!");
        Console.ReadLine();
    } 
    while (input != '1' && input != '2');
}

void Aggregate () 
{
    SqlConnection connection = new SqlConnection(ConnectionString);
    Console.WriteLine("Tabulky:\n1. Games\n2. Game Studio");
    string query = null;
    SqlCommand cmd;
    char input;
    do
    {
        Console.Write("Vyberte tabulku na AGREGAČNÍ FUNKCE (1, 2): ");
        input = Console.ReadKey().KeyChar;
        if (input == '1')
        {
            connection.Open();
            Vypis(new SqlCommand("select * from games", connection));
            connection.Close();
            connection.Open();
            Console.WriteLine("Počet záznamů: " + Convert.ToInt32(new SqlCommand("select count(*) from games", connection).ExecuteScalar()));
            while (true)
            {
                Console.Write("Vyberte sloupec: ");
                string column = Console.ReadLine().ToLower();
                try
                {
                    Console.WriteLine("Maximální hodnota: " + Convert.ToInt32(new SqlCommand($"select max({column}) from games", connection).ExecuteScalar()));
                    Console.WriteLine("Minimální hodnota: " + Convert.ToInt32(new SqlCommand($"select min({column}) from games", connection).ExecuteScalar()));
                    Console.WriteLine("Průměrná hodnota: " + Convert.ToDouble(new SqlCommand($"select avg(cast({column} as float)) from games", connection).ExecuteScalar()));
                    Console.WriteLine("Suma hodnot: " + Convert.ToInt32(new SqlCommand($"select sum({column}) from games", connection).ExecuteScalar()));
                    break;
                }
                catch 
                {
                    Console.WriteLine("U tohoto sloupce nelze využit agregačních funkcí!");
                }
            }
            connection.Close();
        }
        else if (input == '2')
        {
            connection.Open();
            Vypis(new SqlCommand("select * from gamestudio", connection));
            connection.Close();
            connection.Open();
            Console.WriteLine("Počet záznamů: " + Convert.ToInt32(new SqlCommand("select count(*) from gamestudio", connection).ExecuteScalar()));
            while (true)
            {
                Console.Write("Vyberte sloupec: ");
                string column = Console.ReadLine().ToLower();
                try
                {
                    Console.WriteLine("Maximální hodnota: " + Convert.ToInt32(new SqlCommand($"select max({column}) from gamestudio", connection).ExecuteScalar()));
                    Console.WriteLine("Minimální hodnota: " + Convert.ToInt32(new SqlCommand($"select min({column}) from gamestudio", connection).ExecuteScalar()));
                    Console.WriteLine("Průměrná hodnota: " + Convert.ToDouble(new SqlCommand($"select avg(cast({column} as float)) from gamestudio", connection).ExecuteScalar()));
                    Console.WriteLine("Suma hodnot: " + Convert.ToInt32(new SqlCommand($"select sum({column}) from gamestudio", connection).ExecuteScalar()));
                    break;
                }
                catch
                {
                    Console.WriteLine("U tohoto sloupce nelze využit agregačních funkcí!");
                }
            }
        }
        else
            Console.WriteLine("\nTabulka neexistuje!");
        Console.ReadLine();
    }
    while (input != '1' && input != '2');
}
void Vypis(SqlCommand cmd)
{
    const int space = 18;
    SqlDataReader read = cmd.ExecuteReader();
    string text = "";

    string header = "";
    for (int i = 0; i < read.FieldCount; i++)
    {
        if (read.GetName(i) == "Id")
            header += $"| {read.GetName(i),-4}";
        else
            header += $"| {read.GetName(i),-space}";
    }
    text += header + "\n";

    text += new string('¯', header.Length) + "\n";

    while (read.Read())
    {
        string row = "";
        for (int i = 0; i < read.FieldCount; i++)
        {
            string value = read[i].ToString();
            if (read.GetName(i) == "Id")
                row += $"| {value,-4}";
            else
                row += $"| {value,-space}";
        }
        text += row + "\n";
    }

    Console.Clear();
    Console.WriteLine(text);
}