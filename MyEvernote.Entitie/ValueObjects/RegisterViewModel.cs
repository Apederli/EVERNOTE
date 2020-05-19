using MyEvernote.Entitie;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entitie.ValueObjects
{
    public class RegisterViewModel
    {

        [DisplayName("Ad"), Required(ErrorMessage = "{0} Boş geçilemez..!"), StringLength(20, ErrorMessage = ("{1} Karakterden Fazla Yazılamaz..!"))]
        public string Name { get; set; }



        [DisplayName("Soyad"), Required(ErrorMessage = "{0} Boş geçilemez..!"), StringLength(20, ErrorMessage = ("{1} Karakterden Fazla Yazılamaz..!"))]
        public string Surname { get; set; }



        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} Boş geçilemez..!"), StringLength(20, ErrorMessage = ("{1}  Karakterden Fazla Yazılamaz..!"))]
        public string Username { get; set; }


        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} Boş geçilemez..!"), StringLength(30, ErrorMessage = ("{0} adresi {1} Karakterden Fazla Yazılamaz..!")), EmailAddress(ErrorMessage = "E posta adresi doğru formatta değiil")]
        public string EMail { get; set; }


        [DisplayName("Şifre"), Required(ErrorMessage = "{0} Boş geçilemez..!"), StringLength(25, ErrorMessage = ("{0} maks. {1} Karakter Olmalıdır..!"))]
        public string Password { get; set; }


        [DisplayName("Şifre Tekrarı"), Required(ErrorMessage = "{0} Boş geçilemez..!"), StringLength(25, ErrorMessage = ("{0} maks. {1} Karakter Olmalıdır..!")), Compare("Password", ErrorMessage ="{0} ile {1} Uyuşmuyor..!")]
        public string RePassword { get; set; }

    }
}