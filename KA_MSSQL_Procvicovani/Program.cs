using KA_MSSQL_Procvicovani;
using Microsoft.Data.SqlClient;

string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\EDM_PC\Documents\Programy\VS\KA_MSSQL_Procvicovani\KA_MSSQL_Procvicovani\KA_MSSQL_Procvicovani\hry.mdf"";Integrated Security=True;Connect Timeout=30";
// SqlConnection connection = new SqlConnection(ConnectionString);

while (true)
{
    char input;
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
            Aggregation();
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
    while (true)
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
            cmd = new SqlCommand("SELECT Id, Name FROM GameStudio", connection);
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

            break;
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
            break;
        }
        else
        {
            Console.WriteLine("\nTabulka neexistuje!");
        }
    }
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
        else Console.WriteLine("Tento dotaz neobsahuje SELECT!");
    }
    try
    {
        connection.Open();
        Vypis(cmd);
        connection.Close();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    Console.ReadLine();
}
void Update() { }
void Sort() { }
void Delete() { }
void Aggregation () { }
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