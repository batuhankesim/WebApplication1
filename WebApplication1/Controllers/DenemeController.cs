using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Web.Configuration;

namespace WebApplication1.Controllers
{


    public class DenemeController : Controller
    {

        // GET: Deneme
        public ActionResult Index()
        {



            return View();

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(System.Web.HttpPostedFileBase yuklenecekDosya)
        {
            if (yuklenecekDosya != null)
            {
                string constr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
                BinaryReader b = new BinaryReader(yuklenecekDosya.InputStream);
                byte[] binData = b.ReadBytes(yuklenecekDosya.ContentLength);
                int say = 0;
                string result = System.Text.Encoding.UTF8.GetString(binData);
                List<string> satirlar = new List<string>();
                foreach (var satir in result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {

                    say++;
                    if (say > 3)
                    {

                        satirlar.Add(satir);
                    }
                }
                satirlar.RemoveAt(satirlar.Count - 1);


                // List<hareket> hareketler = new List<hareket>();



                using (NpgsqlConnection con = new NpgsqlConnection(constr))
                {
                    string query = "INSERT INTO public.hareket(hislemtarih, hkartno, mtckimlikno,hislemtutariyi,hharekettipi,hislemaciklamasi,hmerchname,iislemadi ) VALUES(@hislemtarih, @hkartno,@mtckimlikno,@hislemtutariyi,@hharekettipi,@hislemaciklamasi,@hmerchname,@iislemadi )";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        hareket h;
                        foreach (var satir in satirlar)
                        {
                            h = new hareket();
                            h.hislemtarih = Convert.ToDateTime(satir.Substring(0, 55).Trim());
                            h.hkartno = Convert.ToInt64(satir.Substring(56, 20).Trim());
                            h.mtckimlikno = Convert.ToInt64(satir.Substring(76, 15).Trim());
                            h.hislemtutariyi = Convert.ToDecimal(satir.Substring(92, 21).Trim().Replace(".", ","));
                            h.hharekettipi = satir.Substring(114, 12).Trim();
                            h.iislemadi = satir.Substring(127, 40).Trim();
                            h.hislemaciklamasi = satir.Substring(168, 40).Trim();
                            h.hmerchname = satir.Substring(209, 20).Trim();

                            if (cmd.Parameters.Count > 0) {

                                cmd.Parameters.Clear();
                            }
                            cmd.Parameters.AddWithValue("@hislemtarih", h.hislemtarih);
                            cmd.Parameters.AddWithValue("@hkartno", h.hkartno);
                            cmd.Parameters.AddWithValue("@mtckimlikno", h.mtckimlikno);
                            cmd.Parameters.AddWithValue("@hislemtutariyi", h.hislemtutariyi);
                            cmd.Parameters.AddWithValue("@hharekettipi", h.hharekettipi);
                            cmd.Parameters.AddWithValue("@hislemaciklamasi", h.hislemaciklamasi);
                            cmd.Parameters.AddWithValue("@hmerchname", h.hmerchname);
                            cmd.Parameters.AddWithValue("@iislemadi", h.iislemadi);
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();


                        //hareketler.Add(h); 
                    }
                }



                //string dosyaYolu = Path.GetFileName(yuklenecekDosya.FileName);
                //var yuklemeYeri = Path.Combine(Server.MapPath("~/Views"), dosyaYolu);
                //yuklenecekDosya.SaveAs(yuklemeYeri);
            }


            return View();

        }

    }
}


    

    
    
    
