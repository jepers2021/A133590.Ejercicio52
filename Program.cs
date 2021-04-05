using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace A133590.Ejercicio52
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ejercicio 52");
            Dictionary<string, string> diccionario = new Dictionary<string, string>();


            while(true)
            {
                Console.Write("> ");
                string comando = Console.ReadLine().Trim();
                if (comando.Length == 0)
                {
                    Console.WriteLine("Comando inválido.");
                    continue;
                }
                string[] cadenasComando = comando.Split();

                switch(cadenasComando[0])
                {
                    case "Alta":
                        {

                            if (cadenasComando.Length < 3)
                            {
                                Console.WriteLine("Comando correcto, error de sintaxis.");
                                break;
                            }

                            string legajo = cadenasComando[1];
                            string nombre = cadenasComando[2];
                            if (diccionario.ContainsKey(legajo))
                            {
                                Console.WriteLine("Legajo ya existente.");
                                continue;
                            }
                            bool exito = true;
                            foreach (char c in legajo) exito &= Char.IsDigit(c);
                            if (!exito)
                            {
                                Console.WriteLine("Legajo debe ser sólo numérico.");
                                continue;
                            }

                            foreach (char c in nombre) exito &= Char.IsLetter(c);
                            if (!exito)
                            {
                                Console.WriteLine("Nombre inválido, debe tener sólo letras.");
                                continue;
                            }


                            diccionario.Add(legajo, nombre);
                            Console.WriteLine($"Agregado legajo: '{legajo}', nombre: '{nombre}'");
                        }

                        break;
                    case "Baja":
                        {
                            if (cadenasComando.Length < 2)
                            {
                                Console.WriteLine("Comando correcto, error de sintaxis.");
                                break;
                            }
                            string legajo = cadenasComando[1];
                            if (!diccionario.ContainsKey(legajo))
                            {
                                Console.WriteLine("Legajo no existe.");
                                continue;
                            }
                            else
                            {
                                diccionario.Remove(legajo);
                                Console.WriteLine("Baja dada con éxito");
                            }
                        }


                        break;
                    case "Grabar":
                        {
                            if (cadenasComando.Length < 2)
                            {
                                Console.WriteLine("Comando correcto, error de sintaxis.");
                                break;
                            }
                            string ruta = cadenasComando[1];

                            FileStream fs = new FileStream(ruta, FileMode.Create);
                            BinaryFormatter bf = new BinaryFormatter();
                            try
                            {
                                bf.Serialize(fs, diccionario);
                                Console.WriteLine($"Grabado exitosamente en: {Path.GetFullPath(ruta)}");
                            }
                            catch(SerializationException e)
                            {
                                Console.WriteLine("No se pudo grabar el archivo.");
                                Console.WriteLine(e.Message);
                                Console.WriteLine(e.StackTrace);
                                continue;
                            }
                            finally
                            {
                                fs.Close();
                            }
                        }
                        break;
                    case "Leer":
                        {
                            if (cadenasComando.Length < 2)
                            {
                                Console.WriteLine("Comando correcto, error de sintaxis.");
                                break;
                            }
                            string ruta = cadenasComando[1];
                            if(!File.Exists(ruta))
                            {
                                Console.WriteLine("Archivo no existente.");
                                continue;
                            }
                            FileStream fs = new FileStream(ruta, FileMode.Open);
                            try
                            {
                                BinaryFormatter formatter = new BinaryFormatter();

                                
                                diccionario = (Dictionary<string,string>)formatter.Deserialize(fs);
                                Console.WriteLine($"Leído exitosamente de {Path.GetFullPath(ruta)}");
                            }
                            catch (SerializationException e)
                            {
                                Console.WriteLine("Fallo al leer el archivo. Razón: " + e.Message);
                                Console.WriteLine(e.StackTrace);
                            }
                            finally
                            {
                                fs.Close();
                            }
                        }
                        break;

                    case "fin":
                        Console.WriteLine("Presione cualquier tecla para continuar..");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Comando inválido. Comandos disponibles: ");
                        Console.WriteLine("Alta [Legajo] [Nombre]");
                        Console.WriteLine("Baja [legajo]");
                        Console.WriteLine("Grabar [ruta]");
                        Console.WriteLine("Leer [ruta]");
                        Console.WriteLine("fin");
                        break;
                }
            }
        }
    }
}
