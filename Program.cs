using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace visarq
{
    class Program
    {
        const string ArrowDown = "ARROWDOWN";
        const string ArrowUp = "ARROWUP";
        const string PageDown = "PAGEDOWN";
        const string PageUp = "PAGEUP";        
        const string Exit = "Exit";
        const string L = "L";

        static int lInf;
        static int lSup;        
        static int j = 0;
        static int iPage = 10;
        static int iBuffer = 100;
        static int iPosicaoArq = 0;
        static List<string> linhas;
        static string _filename;
        
        private static void loadCache(int p_lInf, int p_lSup)
        {
            lInf = p_lInf;
            lSup = p_lSup;
            
            using (StreamReader reader = File.OpenText(_filename))
            {
                int i = 0;
                string linha = "";
                linhas = new List<string>();

                if (i < (iBuffer * j))
                {
                    while (i < iPosicaoArq)
                    {
                        linha = reader.ReadLine();
                        i++;
                    }
                    i = 0;
                }

                while ((linha = reader.ReadLine()) != null && i <= iBuffer)
                {
                    linhas.Add(linha);
                    i++;
                }
            }
        }

        static void imprimirLinha(int lInf, int lSup)
        {
            if (lInf >= 0)
            {
                for (var i = lInf; i <= lSup; i++)
                {
                    Console.WriteLine($"{linhas[i]}\n");
                }
            }
        }

        static void imprimirMenu()
        {
            Console.WriteLine("\nDigite um dos comandos abaixo seguido da tecla ENTER ou EXIT para sair:\n");
            Console.WriteLine("ArrowDown => Avançar uma linha");
            Console.WriteLine("ArrowUp => Retroceder uma linha");
            Console.WriteLine("PageDown => Avançar uma página");
            Console.WriteLine("PageUp => Retroceder uma página");
            Console.WriteLine("L => Exibe 5 linhas anteriores e posteriores a L\n");
        }

        static void Main(string[] args)
        {
            //string filename = @"C:\Projetos\visarq\Arquivos\texto.txt";
            Console.WriteLine("Informe o nome do arquivo: ");
            _filename = Console.ReadLine();

            loadCache(-1, 9);
            imprimirMenu();

            string comando;

            do
            {
                comando = Console.ReadLine();
                Console.WriteLine("\n");

                switch (comando.ToUpper())
                {
                    case ArrowDown:

                        lInf++; lSup++;

                        if (lSup > iBuffer)
                        {
                            j++;
                            iPosicaoArq += lInf;
                            loadCache(-1, 9);

                            Console.WriteLine("\nRecarregou buffer\n");

                            lInf++; lSup++;
                        }

                        break;

                    case ArrowUp:

                        lInf = lInf - 1 < 0 ? 0 : lInf - 1;
                        lSup = lSup - 1 < iPage ? iPage : lSup - 1;

                        if (lInf == 0 && iPosicaoArq > 0)
                        {
                            iPosicaoArq += iPage - iBuffer - 1;
                            lSup = iBuffer;
                            lInf = lSup - iPage;

                            loadCache(lInf, lSup);
                            Console.WriteLine("\nRecarregou buffer\n");
                        }

                        break;

                    case PageDown:

                        lInf = lInf + 1 > 0 ? lSup + 1 : lInf + 1;
                        lSup = lSup + 1 > iPage ? lSup + iPage + 1 : iPage;

                        //if (lSup > linhas.Count)
                        if (linhas.Count < iBuffer)
                        {                            
                            lSup = linhas.Count - 1;
                            lInf = lSup - iPage;
                        }
                        else if (lSup > iBuffer)
                        {
                            j++;
                            iPosicaoArq += lInf;

                            loadCache(-1, 9);
                            Console.WriteLine("\nRecarregou buffer\n");

                            lInf = lInf + 1 > 0 ? lSup + 1 : lInf + 1;
                            lSup = lSup + 1 > iPage ? lSup + iPage + 1 : iPage;
                        }                        
                        break;

                    case PageUp:

                        lInf = lInf > 0 ? lSup - iPage * 2 - 1 : 0;
                        lSup = lSup > iPage ? lSup - iPage - 1 : iPage;

                        if (lInf < 0)
                        {
                            lInf = 0;
                            lSup = iPage;
                            if (iPosicaoArq < iBuffer)
                                iPosicaoArq = 0;
                        }

                        if (lInf == 0 && iPosicaoArq > 0)
                        {
                            j--;
                            if (j < 0)
                                j = 0;

                            iPosicaoArq += iPage - iBuffer;

                            if(iPosicaoArq < iBuffer)
                                iPosicaoArq = 0;

                            lSup = iBuffer;
                            lInf = lSup - iPage;

                            loadCache(lInf, lSup);
                            Console.WriteLine("\nRecarregou buffer\n");
                        }

                        break;

                    case L:

                        Console.Write("Digite a linha a partir da qual deseja visualizar o texto: ");
                        int linha = int.Parse(Console.ReadLine());
                        Console.WriteLine("\n");

                        lInf = linha - 6 <= 0 ? 0 : linha - 6;
                        lSup = linha + 4;

                        break;

                    default:
                        Console.Clear();
                        imprimirMenu();
                        break;
                }
                imprimirLinha(lInf, lSup);
            } while (!comando.ToUpper().Equals("EXIT"));
        }
    }
}
