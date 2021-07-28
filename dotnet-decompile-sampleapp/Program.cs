using System;
using System.Text;

namespace Sample_Application
{
    class Program
    {
        static readonly uint keyAHash = 0xc3a46917; //dotnetdecompile
        static readonly uint keyBHash = 0xe033994c; //cihansol.com
        static uint[] polynomialTbl = new uint[256];

        static void InitCrc32()
        {
            //Build the polynomial Table required by this demo
            for (int i = 0; i < polynomialTbl.Length; i++)
            {
                var tableEntry = (uint)i;
                for (var j = 0; j < 8; ++j)
                {
                    tableEntry = ((tableEntry & 1) != 0)
                        ? (0xEDB88320 ^ (tableEntry >> 1))
                        : (tableEntry >> 1);
                }
                polynomialTbl[i] = tableEntry;
            }        
        }

        static uint HashStr(string str)
        {
            byte[] strData = Encoding.ASCII.GetBytes(str);
            uint result = uint.MaxValue;
            for (int i = 0; i < strData.Length; i++)
            {
                result = (result >> 8) ^ polynomialTbl[(result ^ strData[i]) & 0xFF];
            }
            return ~result;
        }

        static bool CheckInputPP(string ppA)
        {
            var crc32Hash = HashStr(ppA);
            if (crc32Hash == keyAHash || crc32Hash == keyBHash)
                return true;
            else
                return false;
        }

 
        static void ProcessRequest()
        {
            Console.WriteLine("Correct Passphases entered!");
            Console.WriteLine("Processing request.");
        }

        static void Main(string[] args)
        {
            InitCrc32();
            Console.WriteLine("Please enter the required passphrases: A and B");
            Console.WriteLine("Enter Passphrase A:");
            string inputPPA = Console.ReadLine();
            Console.WriteLine("Enter Passphrase B:");
            string inputPPB = Console.ReadLine();

            if (CheckInputPP(inputPPA) && CheckInputPP(inputPPB))
                ProcessRequest();
            else
                Console.WriteLine("Incorrect passphrases entered");

            Console.ReadKey();
        }
    }
}
