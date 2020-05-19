using MyEvernote.BusinessLay;
using MyEvernote.BusinessLay.Results;
using MyEvernote.Entitie;
using MyEvernote.Entitie.Mesagge;
using MyEvernote.Entitie.ValueObjects;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        public ActionResult Index()
        {

            // Database oluşturarak tüm metotları test
            //Test t = new Test();//t.Insert();//t.Update(); //t.Delete();//t.CommentTest();


            //Temp data ile diğer kontrollerdan(Category Controller) notları listeleme
            //if (TempData["mm"] != null)
            //  {
            //      return View(TempData["mm"] as List<Note>);
            //  } 

            
            //C# üzerinde tarihe göre sıralama yaptık  bu listeleme yükü sqle de verilebilir.
            return View(noteManager.List().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            
            Category cat = categoryManager.Find(x=>x.Id == id.Value);

            return View("Index", cat.Notes);
        }

        public ActionResult MostLiked()
        {
            return View("Index", noteManager.List().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerManager<EvernoteUser> res = evernoteUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                   if( res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/123-456-789";
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                Session["Login"] = res.Result;

                return RedirectToAction("Index");
            }


            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerManager<EvernoteUser> res = evernoteUserManager.Registeruser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                else
                {
                    OkViewModel okNotify = new OkViewModel()
                    {
                        Title = "Kayıt Başarılı",
                        RedicrtingUrl="/Home/Login"
                    };
                    okNotify.Items.Add("Lütfen E-Posta Adresinize Gelenaktivasyon Linkine Tıklayarak Hesabınızı Aktif Ediniz.HESABINIZI AKTİF ETMEDEN NOTE EKLEYEMEZ BEĞENİ YAPAMAZSINIZ");

                    return View("Ok", okNotify);
                }
               
            }
            return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerManager<EvernoteUser> res= evernoteUserManager.Active(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotify = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors

                };
                return View("Error", errorNotify);
            }
            OkViewModel okNotify = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedicrtingUrl = "/Home/Login"
            };
            okNotify.Items.Add("Hesabınız Aktifleştirildi Artık Not Paylaşabilir Beğeni yapabilirsiniz.");

            return View("Ok", okNotify);
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = Session["Login"] as EvernoteUser;
            BusinessLayerManager<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotify = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors,

                };
                return View("Error", errorNotify);
            }
            

            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            EvernoteUser currentUser = Session["Login"] as EvernoteUser;
            BusinessLayerManager<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotify = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors

                };
                return View("Error", errorNotify);
            }


            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null && (ProfileImage.ContentType == "image/jpg" || ProfileImage.ContentType == "image/jpeg" || ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.ProfileImageFileName = filename;
                }

                BusinessLayerManager<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel message = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profile Güncellenemedi.",
                        RedicrtingUrl = "/Home/EditProfile",
                        RedirectingTimeout = 5000
                    };
                    return View("Error", message);
                }
                Session["Login"] = res.Result;
                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }

        [Auth]
        public ActionResult DeletProfile(EvernoteUser model)
        {
            EvernoteUser currentUser = Session["Login"] as EvernoteUser;
            BusinessLayerManager<EvernoteUser> res = evernoteUserManager.RemoveUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel message = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedicrtingUrl = "/Home/ShowProfile"
                };
            }
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Test()
        {
            ErrorViewModel model = new ErrorViewModel()
            {
                Header = "Yönlendiriliyorsunuz...",
                Title = "Ok Test",
                RedirectingTimeout = 5000,
                Items= new List<ErrorMessageObj>() { new ErrorMessageObj() { Message="Test1" } }
                
            };
            return View("Error", model);
        }

  

    }
}
