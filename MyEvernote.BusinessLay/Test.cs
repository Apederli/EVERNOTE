using MyEvernote.DataAccessLay.EntityFramework;
using MyEvernote.DataAccessLayer;
using MyEvernote.Entitie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLay
{
    public class Test
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();
        public Test()
        {
           List<Category> cat = repo_category.List();
           List<Category> cat_filter = repo_category.List(x=>x.Id>5);
        }

        public void Insert()
        {
           int result = repo_user.Insert(new EvernoteUser()
            {
               Name = "hhh",
               Surname = "bbb",
               Email = "Dilara@gmail.com",
               ActivateGuid = Guid.NewGuid(),
               IsActive = true,
               IsAdmin = true,
               Username = "abj",
               Password = "111",
               CreatedOn = DateTime.Now,
               ModifiedOn = DateTime.Now.AddMinutes(5),
               ModifiedUsername = "jjj"
           });
        }
        public void Update()
        {
            EvernoteUser user = repo_user.Find(x => x.Username=="abj");

            if (repo_user != null)
            {
                user.Username = "xxx";
                int result = repo_user.Update(user);
            }
        }
        public void Delete()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "xxx");
            if (user != null)
            {
               int result= repo_user.Delete(user);
            }
        }
        public void CommentTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Id == 1);
            Note note = repo_note.Find(x => x.Id == 3);

            Comment com = new Comment()
            {
                Text = "Bu bir Testtir22",
                CreatedOn=DateTime.Now,
                ModifiedOn=DateTime.Now,
                ModifiedUsername="pederli",
                Note=note,
                Owner=user
            };

            repo_comment.Insert(com);
        }
    }
}
