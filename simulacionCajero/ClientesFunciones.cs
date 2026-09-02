using System;
using Microsoft.Data.Sqlite;
using Npgsql;

namespace SeccionCliente
{
    public partial class Program
    {
        private static string CadenaConexion = "Host=localhost;Username=postgres;Password=1234;Database=postgres";

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
                using (var connection = new NpgsqlConnection(CadenaConexion))
                {
                    connection.Open();
                    Console.WriteLine("¡Conexión exitosa a la base de datos PostgreSQL desde C#!");
                }
            }
            catch (Exception ex)
            {
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
                    command.CommandText = "SELECT Id, Nombre FROM Clientes";                    

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            
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
