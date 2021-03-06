﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entitie
{
    [Table("EvernoteUsers")]
    public class EvernoteUser: MyEntitiesBase
    {
        [DisplayName("İsim"), StringLength(25, ErrorMessage ="{0} alanı maksimum {1} karakter olmalıdır")]
        public string Name { get; set; }

        [DisplayName("Soyisim"), StringLength(25, ErrorMessage = "{0} alanı maksimum {1} karakter olmalıdır")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"),StringLength(25, ErrorMessage = "{0} alanı maksimum {1} karakter olmalıdır"), Required(ErrorMessage ="{0} Boş Bırakılamaz")]
        public string Username { get; set; }

        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} Boş Bırakılamaz"), StringLength(70)]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} Boş Bırakılamaz"), StringLength(100)]
        public string Password { get; set; }

        [StringLength(50), ScaffoldColumn(false)]
        public string ProfileImageFileName { get; set; }

        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }


        [Required , ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }
       
        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
