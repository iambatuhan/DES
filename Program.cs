using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Des
{
    public class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            Process currentProc = Process.GetCurrentProcess();
            Program pg = new Program();
            Console.WriteLine("DES ile Şifrelemek için Şifrele veya şifre çözmek için Şifre Çöz Yazınız");
            string password = "password";
            string tercih = Console.ReadLine();
            Console.WriteLine("Bellek Kullanımı: " + currentProc.PrivateMemorySize64);
            if (tercih == "Şifrele")
            {
                
                stopwatch.Start();
                Console.WriteLine("Şifrelenecek metni giriniz");
                string metin = Console.ReadLine();

                string crypto = pg.Encypt(metin, password);
                stopwatch.Stop();
                Console.WriteLine(crypto+" \n Çalışma zamanı"+stopwatch.Elapsed);
            





            }
            else if(tercih=="Şifre Çöz")
            {
                stopwatch.Start();
                Console.WriteLine("Çözülecek Metni Giriniz");
                string metin = Console.ReadLine();
                string crypto = pg.Descrypto(metin, password);
                Console.WriteLine(crypto);
                stopwatch.Stop();
                Console.WriteLine(crypto + " \n Çalışma zamanı" + stopwatch.Elapsed);
            }
            else
            {
                Console.WriteLine("Yanlıs Giriş");
                
            }
            Console.ReadLine();

        }
        public string Encypt(string message, string password)
        {
            byte[] messagebytes = ASCIIEncoding.ASCII.GetBytes(message);//messagebytes byte  dizisi oluşturma 
            byte[] passwordbytes = ASCIIEncoding.ASCII.GetBytes(password);//passwordbytes  byte  dizisi oluşturma 
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();//DESCryptoServiceProvider sınıfını değişkene atama
            ICryptoTransform transform = provider.CreateEncryptor(passwordbytes, passwordbytes);///Simetrik Veri Şifreleme Standardı ( oluştururDES) belirtilen anahtar ( ile şifreleme nesnesiAnahtar) ve başlatma vektörü (IV).
            CryptoStreamMode mode = CryptoStreamMode.Write;
            MemoryStream stream = new MemoryStream();
            CryptoStream crypto = new CryptoStream(stream, transform, mode);
            crypto.Write(messagebytes, 0, messagebytes.Length);
            crypto.FlushFinalBlock();//Temel alınan veri kaynağını veya depoyu arabelleğin geçerli durumuyla güncelleştirir.
            byte[] encyptedMessagBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(encyptedMessagBytes, 0, encyptedMessagBytes.Length);
            string encyptMessage = Convert.ToBase64String(encyptedMessagBytes);
            return encyptMessage;



        }
        public string Descrypto(string encryptedMessage, string password)
        {
            byte[] encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
            byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(password);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
            CryptoStreamMode mode = CryptoStreamMode.Write;
            MemoryStream memory = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memory, transform, mode);
            cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] decryptoMessageBytes = new byte[memory.Position];
            memory.Position = 0;
            memory.Read(decryptoMessageBytes, 0, decryptoMessageBytes.Length);
            string message = ASCIIEncoding.ASCII.GetString(decryptoMessageBytes);
            return message;



        }
    }
}