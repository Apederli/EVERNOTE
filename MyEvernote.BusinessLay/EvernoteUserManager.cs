using Myevernote.Common.Helpers;
using MyEvernote.BusinessLay.Abstract;
using MyEvernote.BusinessLay.Results;
using MyEvernote.DataAccessLay.EntityFramework;
using MyEvernote.Entitie;
using MyEvernote.Entitie.Mesagge;
using MyEvernote.Entitie.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLay
{
    public class EvernoteUserManager : ManagerBase<EvernoteUser>
    {
        BusinessLayerManager<EvernoteUser> res = new BusinessLayerManager<EvernoteUser>();
        EvernoteUser user;

        public BusinessLayerManager<EvernoteUser> Registeruser(RegisterViewModel data)
        {

            user = Find(x => x.Username == data.Username || x.Email == data.EMail);
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Bu Kullanıcı Adı Kullanılıyor");
                }

                if (user.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Bu E-Posta Adresi Kullanılıyor");
                }

                return res;
            }
            else
            {
                int dbResult = base.Insert(new EvernoteUser()
                {
                    Name = data.Name,
                    Surname = data.Surname,
                    Username = data.Username,
                    Email = data.EMail,
                    ActivateGuid = Guid.NewGuid(),
                    Password = data.Password,
                    ProfileImageFileName = "DefoultImage.png"

                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Username == data.Username && x.Email == data.EMail);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{ siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Hesabınızı Aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";
                    MailHelper.SendMail(body, res.Result.Email, "My Evernote Hesap Aktifleştirme");
                }

            }

            return res;
        }

        public BusinessLayerManager<EvernoteUser> LoginUser(LoginViewModel model)
        {
            user = Find(x => x.Username == model.Username && x.Password == model.Password);
            res.Result = user;
            if (user != null)
            {
                if (!user.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Hesap Aktif Değil Lütfen E-POSTA adresinize gelen doğrulama linkine  tıklayarak hesabınızı aktif edin...");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen E-Posta Adresinizi Kontrol Ediniz..!");
                }

            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı Adı veya Şifre Hatalı");
            }

            return res;
        }

        public BusinessLayerManager<EvernoteUser> Active(Guid g)
        {
            res.Result = Find(x => x.ActivateGuid == g);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivedIdDoesNotExist, "Aktifleştirilecek Kulanıcı Bulunamadı");
            }
            return res;
        }

        public BusinessLayerManager<EvernoteUser> GetUserById(int id)
        {
            res.Result = Find(x => x.Id == id);
            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.CheckYourEmail, "Kullanıcı Bulunamadı");
            }
            return res;
        }

        public BusinessLayerManager<EvernoteUser> UpdateProfile(EvernoteUser model)
        {
            user = Find(x => x.Username == model.Username || x.Email == model.Email);
            if (user != null && user.Id != model.Id)
            {
                if (user.Username == model.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı..!");
                }

                if (user.Email == model.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E- Posta Adresi Kullanılıyor..!");
                }

                return res;
            }

            res.Result = Find(x => x.Id == model.Id);
            res.Result.Email = model.Email;
            res.Result.Name = model.Name;
            res.Result.Surname = model.Surname;
            res.Result.Username = model.Username;
            res.Result.Password = model.Password;
            if (string.IsNullOrEmpty(model.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = model.ProfileImageFileName;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdate, "Profil Güncellenemedi");
            }

            return res;
        }

        public BusinessLayerManager<EvernoteUser> RemoveUserById(int id)
        {
            user = Find(x => x.Id == id);
            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNoteRemove, "Kullanıcı Silinemedi");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Silinemedi");
            }

            return res;
        }


        public new BusinessLayerManager<EvernoteUser> Insert(EvernoteUser data)
        {

            res.Result = data;
            user = Find(x => x.Username == data.Username || x.Email == data.Email);
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Bu Kullanıcı Adı Kullanılıyor");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Bu E-Posta Adresi Kullanılıyor");
                }

                return res;
            }
            else
            {
                res.Result.ProfileImageFileName = "DefoultImage.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Bu E-Posta Adresi Kullanılıyor");
                }


            }

            return res;
        }

        public new BusinessLayerManager<EvernoteUser> Update(EvernoteUser model)
        {
            user = Find(x => x.Username == model.Username || x.Email == model.Email);
            if (user != null && user.Id != model.Id)
            {
                if (user.Username == model.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı..!");
                }

                if (user.Email == model.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E- Posta Adresi Kullanılıyor..!");
                }

                return res;
            }

            res.Result = Find(x => x.Id == model.Id);
            res.Result.Email = model.Email;
            res.Result.Name = model.Name;
            res.Result.Surname = model.Surname;
            res.Result.Username = model.Username;
            res.Result.Password = model.Password;
            res.Result.IsActive = model.IsActive;
            res.Result.IsAdmin = model.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdate, "Profil Güncellenemedi");
            }

            return res;
        }
    }
}

