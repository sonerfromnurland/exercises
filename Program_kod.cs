using System;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp2
{
    internal class Program
    {
        // Allowed hardware IDs (plain for now, so it actually works)
        /*
        private static readonly string[] AllowedHardwareIds =
        {
            "148B640FC9FFE81B5BB9DC570C1BC08F12C469959BE5988ACE0169FC2C72E13A",
            "FF133CD63CEE837603D60B678946143921B8B35E0D5C87D84BFFD28E66D00E88" //benim makine id
        };
        */
        // REPLACE the old AllowedHardwareIds field with this:
        private static readonly string[] AllowedHardwareIds = BuildAllowedHardwareIds();

        private static string[] BuildAllowedHardwareIds()
        {
            // These are NOT your HWIDs directly.
            // They are Base64 of (UTF8 bytes of HWID XOR 0x5A).

            // Obfuscated form of:
            // 148B640FC9FFE81B5BB9DC570C1BC08F12C469959BE5988ACE0169FC2C72E13A
            // FF133CD63CEE837603D60B678946143921B8B35E0D5C87D84BFFD28E66D00E88

            string obf1 = "a25iGGxuahwZYxwcH2JrGG8YGGMeGW9tahlrGBlqYhxraBlubGNjb2MYH29jYmIbGR9qa2xjHBloGW1oH2tpGw==";
            string obf2 = "HBxraWkZHmxpGR8fYmltbGppHmxqGGxtYmNubGtuaWNoaxhiGGlvH2oebxlibR5ibhgcHB5oYh9sbB5qah9iYg==";

            string id1 = DecodeHardwareId(obf1);
            string id2 = DecodeHardwareId(obf2);

            return new[] { id1, id2 };
        }

        private static string DecodeHardwareId(string base64)
        {
            byte[] data = Convert.FromBase64String(base64);

            // same mask used to obfuscate
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ 0x5A);
            }

            return Encoding.UTF8.GetString(data);
        }

        [STAThread]
        private static void Main(string[] args)
        {
            PrintBanner();

            // .NET Framework 4.8 runs only on Windows, so hardware check always applies
            string hwid = GetHardwareId();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Magenta;

            if (!AllowedHardwareIds.Contains(hwid, StringComparer.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Bu programi sadece Fettah Yilmaz kullanabilir.");
                Console.WriteLine("HardwareId = " + hwid);
                Console.ResetColor();
                return;
            }

            Console.WriteLine("3DES Decrypt Console for Fettah Yilmaz");
            Console.ResetColor();
            Console.WriteLine("Type base64 data to decrypt.");
            Console.WriteLine("Press 'E' or type 'exit' to quit.\n");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input != null) input = input.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.Equals("e", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                try
                {
                    string plain = TripleDesDecrypt(input);

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[PLAIN] ");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(plain + "\n");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: " + ex.Message + "\n");
                    Console.ResetColor();
                }
            }
        }

        // 3DES ECB / NoPadding decryption
        private static string TripleDesDecrypt(string base64Cipher)
        {
            byte[] keyBytes = GetKeyBytes();  // obfuscated key
            byte[] cipherBytes = Convert.FromBase64String(base64Cipher);

            using (var tdes = TripleDES.Create())
            {
                tdes.Key = keyBytes;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.None;

                using (var decryptor = tdes.CreateDecryptor())
                {
                    byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
        }

        // Extract original key ABCDEF1234567890ABCDEF1234567890 
        // (obfuscated using XOR & index scrambling)
        private static byte[] GetKeyBytes()
        {
            // Precomputed obfuscated bytes (length=16)
            byte[] obf = new byte[]
            {
                94, 119, 40, 167, 238, 94, 248, 108,
                83, 69, 242, 120, 7, 139, 63, 81
            };

            byte[] temp = new byte[obf.Length];

            for (int i = 0; i < obf.Length; i++)
            {
                temp[i] = (byte)(obf[i] ^ i ^ 0xA5);
            }

            Array.Reverse(temp);
            return temp; // final key bytes
        }

        // HARDWARE ID MATCH (SHA256(cpu + bios + disk))
        private static string GetHardwareId()
        {
            string cpu = GetWmi("Win32_Processor", "ProcessorId");
            string bios = GetWmi("Win32_BIOS", "SerialNumber");
            string disk = GetWmi("Win32_PhysicalMedia", "SerialNumber");

            string combined = cpu + "-" + bios + "-" + disk;
            byte[] bytes = Encoding.UTF8.GetBytes(combined);

            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(bytes);
                // uppercase hex without separators, same as Convert.ToHexString
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        private static string GetWmi(string wmiClass, string property)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT " + property + " FROM " + wmiClass))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        object val = obj[property];
                        if (val != null)
                            return val.ToString().Trim();
                    }
                }
            }
            catch
            {
                // ignore and fall through to "unknown"
            }

            return "unknown";
        }

        private static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(@"  _________      ____ ___ Payment Systems 2026 .__            _____   __                  ");
            Console.WriteLine(@" /   _____/     |    |   \________ ____ ___.__.|__|______    /  _  \ |  | _______ _______ ");
            Console.WriteLine(@" \_____  \      |    |   /\___   // __ <   |  ||  \_  __ \  /  /_\  \|  |/ /\__  \\_  __ \");
            Console.WriteLine(@" /        \     |    |  /  /    /\  ___/\___  ||  ||  | \/ /    |    \    <  / __ \|  | \/");
            Console.WriteLine(@" /_______  / /\ |______/  /_____ \\___  > ____||__||__|    \____|__  /__|_ \(____  /__|   ");
            Console.WriteLine(@"         \/  \/               \/    \/\/                         \/     \/     \/        ");

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}