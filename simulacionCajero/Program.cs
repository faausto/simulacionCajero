using System;
using Microsoft.Data.Sqlite; // Utiliza la libreria del sistema
using SeccionCliente;
using Npgsql;

namespace TerminalPrincipal // Define el espacio de nombres TerminalPrincipal
{

    class Sucursal
    {
        public int Id { get; set; } = 0; // Propiedad para el ID de la sucursal
        public string Nombre { get; set; } = String.Empty;
    }

    class Program // Define la clase Program
    {
        private static string CadenaConexion = "Data Source=database\\Banco.sqlite"; // Cadena de conexión a la base de datos SQLite

        public static List<Sucursal> sucursales = new List<Sucursal>(); // Lista para almacenar las sucursales 

        static void Main(string[] args) // Método principal que se ejecuta al iniciar el programa
        {            
            int opcion = 0;

            sucursales.Add(new Sucursal() { Id = 1, Nombre = "Edo Castex"});

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nPrograma ABML Sucursales"); // Muestra un mensaje de bienvenida en la consola
                // Aquí puedes agregar más código para la funcionalidad de la terminal

                Console.WriteLine("[1] Ingresar Sucursal (Alta)");            
                Console.WriteLine("[2] Modicar Sucursal (Modificar)");
                Console.WriteLine("[3] Borrar Sucursal (Borrar))");
                Console.WriteLine("[4] Listar Sucursales (Listar)");
                Console.WriteLine("[5] Clientes");
                Console.WriteLine("[0] Salir");

                Console.Write("Ingrese una opción: "); // Solicita al usuario que ingrese una opción
                opcion = Convert.ToInt32(Console.ReadLine()); // Lee la opción ingresada por el usuario y la convierte a entero                

                if (opcion == 0)
                    break;

                switch (opcion)
                {                    
                    case 1:
                        /*Console.WriteLine("\n Ingresar Sucursal \n");
                        var nombreSucursal = SolicitarCadena("Ingrese el nombre de la sucursal: ");

                        var nuevaSucursal = new Sucursal
                        {
                            Id = sucursales.Count + 1,
                            Nombre = nombreSucursal
                        }; 

                        sucursales.Add(nuevaSucursal); // Agrega la nueva sucursal a la lista      */   
                        NuevaSucursalBD();
                        break;
                    case 2:                    
                        Console.WriteLine("\n Modificar Sucursal \n");

                        Console.WriteLine("Id Sucursal >");
                        int index = Convert.ToInt32(Console.ReadLine());

                        bool existe = false;
                        foreach (var sucursal in sucursales)
                        {
                            if (sucursal.Id == index)
                            {
                                existe = true;
                                Console.WriteLine($"Sucursal encontrada: {sucursal.Nombre}");
                                var nombre = SolicitarCadena("Ingrese el nuevo nombre de la sucursal: ");

                                sucursal.Nombre = nombre;
                            }
                        }

                        if (!existe)
                        {
                            Console.WriteLine("Sucursal no encontrada");
                        }                        

                        break;
                    case 3:
                        Console.WriteLine("\n Borrar Sucursal \n");
                        BorrarSucursal();

                        break;
                    case 4:
                        Console.WriteLine("\n Listar Sucursales \n");
                        ListarSucursalesBD();

                        break;
                    case 5:
                        SeccionCliente.Program.MenuClientes();
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

        private static void ListarSucursales()
        {
            Console.WriteLine("Identificador \t Nombre ");
            Console.WriteLine();
            foreach (var sucursal in sucursales)
            {
                Console.WriteLine($"{sucursal.Id} \t {sucursal.Nombre}");
            }

            Console.Write("Presione una tecla para continuar");
            Console.ReadKey();
        }

        private static void BorrarSucursal()
        {

            Console.Write("Ingrese el Id de la sucursal a borrar: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Sucursal? sucursalEncontrada = null;

            foreach (var sucursal in sucursales)
            {
                if (sucursal.Id == id)
                {
                    sucursalEncontrada = sucursal;
                    break;
                }
            }

            if (sucursalEncontrada != null)
            {
                sucursales.Remove(sucursalEncontrada);
                Console.WriteLine("Sucursal eliminada correctamente.");
            }
            else
            {
                Console.WriteLine("Sucursal no encontrada.");
            }

            Console.Write("Presione una tecla para continuar");
            Console.ReadKey();
        }

        private static void NuevaSucursalBD()
        {
            Console.WriteLine("\n Ingresar Sucursal \n");

            var nombreSucursal = SolicitarCadena(
                "Ingrese el nombre de la sucursal: "
            );

            using (var connection = new SqliteConnection(CadenaConexion))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = 
                        "INSERT INTO Sucursales (Nombre) VALUES ($nombre)";

                    command.Parameters.AddWithValue("$nombre", nombreSucursal);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Sucursal agregada correctamente.");

            Console.Write("Presione una tecla para continuar");
            Console.ReadKey();
        }

        private static void ListarSucursalesBD()
        {
            Console.WriteLine("Identificador \t Nombre ");
            Console.WriteLine();
            using (var connection = new SqliteConnection(CadenaConexion))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    // Assign the select query to CommandText
                    command.CommandText = "SELECT Id, Nombre FROM Sucursales";                    

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Access data using standard column names or indexing
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            
                            /*
                            var sucursal = new Sucursal
                            {
                                Id = id,
                                Nombre = name
                            };*/
                            Console.WriteLine($"{id} \t {name}");
                        }
                    }
                }
            }          

            Console.Write("Presione una tecla para continuar");
            Console.ReadKey();
        }
    }


}

