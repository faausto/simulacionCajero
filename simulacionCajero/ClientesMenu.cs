using System;

namespace SeccionCliente
{
    public partial class Program
    {
        public static void MenuClientes()
        {            
            int opcion = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nPrograma ATM BANCO VINTAJE\n");
                Console.WriteLine("[1] Cargar Cliente (Alta)");            
                Console.WriteLine("[2] Modicar Cliente (Modificar)");
                Console.WriteLine("[3] Eliminar Cliente (Borrar))");
                Console.WriteLine("[4] Listar Clientes (Listar)");
                Console.WriteLine("[5] Comprobar conexion con base de datos postgres");
                Console.WriteLine("[0] Regresar");

                Console.Write("Ingrese una opción: ");
                opcion = Convert.ToInt32(Console.ReadLine());                

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
    }
}
