using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace FTPDosyaGonderme_Upload_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool FtpUpload(string ipAdresi, string port, string kullaniciAdi, string parola)
        {
            try
            {
                //Göndereceğimiz dosyanın bilgisini FileInfo tipinde bir değişkende tutuyoruz
                FileInfo dosyaBilgisi = new FileInfo(@"D:\ftpupload.txt");
                //Dosyamızı göndermek istediğimiz FTP adresini string tipinde bir değişkende tanımlıyoruz
                string ftpAdresi = "ftp://" + ipAdresi + ":" + port + "/" + dosyaBilgisi.Name;

                //Dosyamızı göndermek istediğimiz IP adresi bilgisini "ftpAdresi" adlı bir string değişkene
                //atamıştık şimdi bu adrese dosyamızı gönder isteğimizi FtpWebRequest tipinde bir nesne oluşturuyoruz.
                FtpWebRequest ftpIstek = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpAdresi));
                //burada ftpIstek nesnemize FTP'ye login olabilmemiz için FTP kullanıcı adı ve parola bilgilerini
                //tanımlarız
                ftpIstek.Credentials = new NetworkCredential(kullaniciAdi, parola);
                //Aynı bağlantının yeniden kullanılamayacağı anlamına gelir, büyük boyutta veri aktarımı
                //yapılacağı zaman timeout değeri yetersiz kaldığı zamanlar true yapılabilir
                ftpIstek.KeepAlive = false;
                //ftp adresi olarak tanımladığımız FTP adresinin içinden dosya mı indireceğiz yoksa oraya dosya mı
                //göndereceğiz bunu belirtiyoruz.
                ftpIstek.Method = WebRequestMethods.Ftp.UploadFile;
                //UseBinary özelliği ile dosyamızı binary formatında göndereceğimizi belirtiyoruz
                ftpIstek.UseBinary = true;
                //gönderilecek veri uzunluğunu göndereceğimiz dosya uzunluğuna eşitliyoruz
                ftpIstek.ContentLength = dosyaBilgisi.Length;

                //dosyamızı byte formatına çevirme kısmı
                int bufferUzunlugu = 2048;
                byte[] buff = new byte[10000000];
                int sayi;
                FileStream stream = dosyaBilgisi.OpenRead();
                Stream str = ftpIstek.GetRequestStream();
                sayi = stream.Read(buff, 0, bufferUzunlugu);
                while (sayi != 0)
                {
                    str.Write(buff, 0, sayi);
                    sayi = stream.Read(buff, 0, bufferUzunlugu);
                }

                str.Close();
                stream.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata mesajı : " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
