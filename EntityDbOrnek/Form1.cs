using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EntityDbOrnek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // globalde entity nesnemizi ürettik
        DbSinavOgrenciEntityEntities db = new DbSinavOgrenciEntityEntities();

        private void BtnDersListesi_Click(object sender, EventArgs e)
        {
            // ADONET İLE listeleme işlemi 
            SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-M4Q4SMD\SQLEXPRESS;Initial Catalog=DbSinavOgrenciEntity;Integrated Security=True");
            SqlCommand komut = new SqlCommand("SELECT * FROM TBLDERSLER", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt; // listeyi görüntüle



        }

        private void BtnOgrenciListele_Click(object sender, EventArgs e)
        {
            // 2 satır ile listeleme işlemini gerçekleştirdik
            // model ile globalde nesne türetip listeleme işlemini gerçekleştirdik
            dataGridView1.DataSource = db.TBLOGRENCI.ToList();

            dataGridView1.Columns[3].Visible = false; // fotoğraflar sütunu görünmez
            dataGridView1.Columns[4].Visible = false; //    TBL NOTLAR ALANI GÖRÜNMEZ
        }

        private void BtnNotListesi_Click(object sender, EventArgs e)
        {
            // istediğimiz sutunları getirme işlemi item değişken ismi 
            var query = from item in db.TBLNOTLAR
                        select new { item.NOTID, item.OGR, item.DERS, item.SINAV1, item.SINAV2, item.SINAV3, item.ORTALAMA, item.DURUM };
            dataGridView1.DataSource = query.ToList();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            // sınıftan nesne ürettik
            TBLOGRENCI t = new TBLOGRENCI();
            t.AD = TxtOgrenciAd.Text;
            t.SOYAD = TxtOgrenciSoyad.Text;
            db.TBLOGRENCI.Add(t); // sınıfımıza girilen değerleri ekleme işlemini gerçekleştir.
            db.SaveChanges(); // değişiklikleri kaydet
            MessageBox.Show("ÖĞRENCİ LİSTEYE EKLENMİŞTİR");
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrenciId.Text); // öğrenciid text den gelen id tut
            var x = db.TBLOGRENCI.Find(id); // x e id değierni bul ve ata
            db.TBLOGRENCI.Remove(x); // değeri sil
            db.SaveChanges();
            MessageBox.Show("Silme işlemi gerçekleştirildi");
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrenciId.Text); // öğrenciid text den gelen id tut
            var x = db.TBLOGRENCI.Find(id); // x e id değierni bul ve ata
            x.AD = TxtOgrenciAd.Text; // seçilen x değerine ait ad soyad ve foto değerlini değiştir
            x.SOYAD = TxtOgrenciSoyad.Text;
            x.FOTOGRAF = TxtOgrenciFoto.Text;

            db.SaveChanges();
            MessageBox.Show("GUNCELLEME İŞLEMİ TAMAMLANDI");
        }

        private void BtnProcedure_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI(); // procedure kullanarak not listesini aldık
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            // x burda değişken ismi ne oldugu farketmez
            // ad txt ten gelen ad değerine eşit mi bul 
            // ad ve soyad değerlerine göre bulma işlemini gerçekleştirir
            dataGridView1.DataSource = db.TBLOGRENCI.Where(x => x.AD == TxtOgrenciAd.Text & x.SOYAD== TxtOgrenciSoyad.Text).ToList();

        }

        private void TxtOgrenciAd_TextChanged(object sender, EventArgs e)
        {
            // girilen textbox degerinde yazılanları arama işlemi gerçekleştiriyor
            string aranan = TxtOgrenciAd.Text;
            var degerler = from item in db.TBLOGRENCI
                           where item.AD.Contains(aranan) 
                           select item;
            dataGridView1.DataSource = degerler.ToList();// liste olarak göster
        }

        private void BtnLinq_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                // öğrenci adlarını a dan z ye sırala  OrdeyBy
                // öğrenci z den a sıralama OrdeyByDescending
                List<TBLOGRENCI> listele = db.TBLOGRENCI.OrderBy(p => p.AD).ToList();
                dataGridView1.DataSource = listele;
            }
            if (radioButton2.Checked == true)
            {
                // ilk 3 eleman dönder take 
                List<TBLOGRENCI> listele = db.TBLOGRENCI.OrderBy(p => p.AD).Take(3).ToList();
                dataGridView1.DataSource = listele;
            }


        }
    }
}
