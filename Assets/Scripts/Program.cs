using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PuntoDeVenta
{
    /*
    public class Producto
    {
        public string Codigo; 
        public string Nombre;
        public decimal Precio;
        public int Cantidad;
    }
    class Program
    {
        static void Main(string[] args)
        {
            string archivoCSV = "productos.csv";
            List<Producto> inventario = CargarInventarioDesdeCSV(archivoCSV);

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine("1. Mostrar inventario");
                Console.WriteLine("2. Agregar producto");
                Console.WriteLine("3. Comprar producto");
                Console.WriteLine("4. Salir");

                string opcionSeleccionada = Console.ReadLine();

                switch (opcionSeleccionada)
                {
                    case "1":
                        Console.Clear();
                        MostrarInventario(inventario);
                        Console.WriteLine("Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.Clear();
                        AgregarProducto(inventario, archivoCSV);
                        break;
                    case "3":
                        Console.Clear();
                        ComprarProducto(inventario, archivoCSV);
                        break;
                    case "4":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }

            Console.WriteLine("¡Hasta luego!");
        }

        static void ComprarProducto(List<Producto> inventario, string archivo)
        {
            Console.WriteLine("Se ha seleccionado la opcion comprar");
        }

        static void ActualizarInventario()
        {
            Console.WriteLine("Se ha seleccionado la opcion comprar");
        }

        static List<Producto> CargarInventarioDesdeCSV(string archivoCSV)
        {
            List<Producto> inventario = new List<Producto>();

            if (File.Exists(archivoCSV))
            {
                string[] lineas = File.ReadAllLines(archivoCSV);
                foreach (string linea in lineas)
                {
                    string[] valores = linea.Split(",");
                    Producto producto = new Producto
                    {
                        Codigo = valores[0],
                        Nombre = valores[1],
                        Precio = decimal.Parse(valores[2]),
                        Cantidad = int.Parse(valores[3])
                    };
                    inventario.Add(producto);
                }
            }
            else
            {
                Console.WriteLine("El archivo de inventario no existe.");
            }

            return inventario;
        }



        static void MostrarInventario(List<Producto> inventario)
        {
            Console.WriteLine("Inventario:");
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("| {0,-10} | {1,-30} | {2,-10} | {3,-10} |", "Código", "Nombre", "Precio", "Cantidad");
            Console.WriteLine("--------------------------------------------------------------------");
            foreach (Producto producto in inventario)
            {
                Console.WriteLine("| {0,-10} | {1,-30} | {2,-10:C} | {3,-10} |", producto.Codigo, producto.Nombre, producto.Precio, producto.Cantidad);
            }
            Console.WriteLine("--------------------------------------------------------------------");
        }

        static void AgregarProducto(List<Producto> inventario, string archivoCSV)
        {
            Console.WriteLine("Agregar producto:");
            Console.Write("Código del producto: ");
            string codigoProducto = Console.ReadLine();

            if (inventario.Any(producto => producto.Codigo == codigoProducto))
            {
                Console.WriteLine("Ya existe un producto con ese código.");
            }
            else
            {
                Console.Write("Nombre del producto: ");
                string nombreProducto = Console.ReadLine();

                Console.Write("Precio del producto: ");
                string precioProducto = Console.ReadLine();
                decimal.TryParse(precioProducto, out decimal precio);
                Console.Write("Cantidad del producto: ");
                int cantidadProducto = Convert.ToInt32(Console.ReadLine());

                Producto productoEncontrado = null;
                foreach (Producto producto in inventario)
                {
                    if (producto.Nombre == nombreProducto)
                    {
                        productoEncontrado = producto;
                        break;
                    }
                }

                if (productoEncontrado == null)
                {
                    Console.WriteLine("El producto no existe en el inventario.");
                }
                else if (productoEncontrado.Cantidad < cantidadProducto)
                {
                    Console.WriteLine($"La cantidad de {productoEncontrado.Nombre} en inventario es insuficiente.");
                }
                else
                {
                    double totalVenta = productoEncontrado.Precio * cantidadProducto;
                    Console.WriteLine($"Total de la venta: ${totalVenta}");

                    productoEncontrado.Cantidad -= cantidadProducto;
                    ActualizarInventario();

                    Venta venta = new Venta(productoEncontrado, cantidadProducto, totalVenta, DateTime.Now);
                    ventas.Add(venta);
                    ActualizarVentas();

                    Console.WriteLine("Venta realizada exitosamente.");
                }
            }
        }
    }
    */
}