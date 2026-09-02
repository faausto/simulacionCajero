using System;
using Microsoft.Data.Sqlite; // Utiliza la libreria del sistema
using Npgsql;

namespace SeccionCliente_v2 // Define el espacio de nombres SeccionCliente
{

    class Cliente
    {
        public int Id { get; set; } = 0; // Propiedad para el ID de la sucursal
        public string Nombre { get; set; } = String.Empty;
    }

    class Program // Define la clase Program
    {
        private static string CadenaConexion = "Host=localhost;Username=postgres;Password=lerolero123;Database=postgres"; // Cadena 

        public static void MenuClientes() // Método para mostrar menu y funciones.
        {            
            int opcion = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nPrograma ATM BANCO\n"); // Muestra un mensaje de bienvenida en la consola
                // Aquí puedes agregar más código para la funcionalidad de la terminal

                Console.WriteLine("[1] Cargar Cliente (Alta)");            
                Console.WriteLine("[2] Modicar Cliente (Modificar)");
                Console.WriteLine("[3] Eliminar Cliente (Borrar))");
                Console.WriteLine("[4] Listar Clientes (Listar)");
                Console.WriteLine("[5] Comprobar conexion con base de datos postgres");
                Console.WriteLine("[0] Regresar");

                Console.Write("Ingrese una opción: "); // Solicita al usuario que ingrese una opción
                opcion = Convert.ToInt32(Console.ReadLine()); // Lee la opción ingresada por el usuario y la convierte a entero                

                if (opcion == 0)
                    break;

                switch (opcion)
                {                    
                    case 1:
                        CargarClienteBD();
                        break;
                    case 2:                    
                        ModificarClienteBD();                   

                        break;
                    case 3:
                        EliminarClienteBD();

                        break;
                    case 4:
                        Console.WriteLine("\n Listar Clientes \n");
                        ListarClientesBD();

                        break;
                    case 5:
                        TestBD();

                        break;
                    default:
                        Console.WriteLine("Invalido");
                        break;
                }
            }   
        

        }

        static string SolicitarCadena(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine();
        }

        private static void TestBD()
        {
            Console.Clear();
            Console.WriteLine("\n Comprobando conexion con base de datos \n");

            try
            {
                // Inicializa la conexión
                using (var connection = new NpgsqlConnection(CadenaConexion))
                {
                    // Intenta abrir la conexión
                    connection.Open();
                    
                    // Si llega acá, la conexión fue exitosa
                    Console.WriteLine("¡Conexión exitosa a la base de datos PostgreSQL desde C#!");
                }
            }
            catch (Exception ex)
            {
                // Si salta una excepción, muestra el mensaje de error
                Console.WriteLine("No se pudo realizar la conexión con la base de datos.");
                Console.WriteLine($"Error detectado: {ex.Message}");
            }

            Console.Write("\n\nPresione una tecla para continuar");
            Console.ReadKey();

        }
        private static void CargarClienteBD()
        {
            Console.Clear();
            Console.WriteLine("\n Cargar Cliente \n");

            var nombreCliente = SolicitarCadena(
                "Ingrese el nombre del cliente: "
            );

            using (var connection = new NpgsqlConnection(CadenaConexion))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = 
                        "INSERT INTO clientes (Nombre) VALUES (@nombre)";

                    command.Parameters.AddWithValue("@nombre", nombreCliente);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Cliente agregado correctamente.");

            Console.Write("Presione una tecla para continuar");
            Console.ReadKey();
        }

        private static void ListarClientesBD()
        {
            Console.Clear();
            Console.WriteLine("\nListar Clientes\n");

            Console.WriteLine("Identificador \t Nombre ");
            Console.WriteLine();
            using (var connection = new NpgsqlConnection(CadenaConexion))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    // Assign the select query to CommandText
                    command.CommandText = "SELECT Id, Nombre FROM Clientes";                    

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Access data using standard column names or indexing
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            
                            /*
                            var cliente = new Cliente
                            {
                                Id = id,
                                Nombre = name
                            };*/
                            Console.WriteLine($"\t{id} \t {name}");
                        }
                    }
                }
            }          

            Console.Write("Presione una tecla para continuar");
            Console.ReadKey();
        }

        private static void ModificarClienteBD()
        {
            Console.Clear();
            Console.WriteLine("\nModificar Cliente\n");

            Console.Write("Id Cliente > ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Ingrese el nuevo nombre del cliente: ");
            string nombre = Console.ReadLine() ?? string.Empty;

            using (var connection = new NpgsqlConnection(CadenaConexion))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        UPDATE Clientes
                        SET Nombre = @nombre
                        WHERE Id = @id";

                    command.Parameters.AddWithValue("$id", id);
                    command.Parameters.AddWithValue("$nombre", nombre);

                    int filasAfectadas = command.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                        Console.WriteLine("Cliente modificado correctamente.");
                    else
                        Console.WriteLine("Cliente no encontrado.");
                }
            }

            Console.Write("Presione una tecla para continuar");
            Console.ReadKey();
        }

        private static void EliminarClienteBD()
        {
            Console.Clear();
            Console.WriteLine("\nEliminar Cliente\n");

            Console.Write("Id Cliente > ");
            int id = Convert.ToInt32(Console.ReadLine());

            using (var connection = new NpgsqlConnection(CadenaConexion))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Clientes WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    int filasAfectadas = command.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                        Console.WriteLine("Cliente eliminado correctamente.");
                    else
                        Console.WriteLine("Cliente no encontrado.");
                }
            }

            Console.Write("Presione una tecla para continuar");
            Console.ReadKey();
        }


    }


}