using MyEvernote.BusinessLay.Abstract;
using MyEvernote.DataAccessLay.EntityFramework;
using MyEvernote.Entitie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLay
{
    public class CategoryManager :ManagerBase<Category>
    {
        private NoteManager noteManager = new NoteManager();
        private LikeManager likeManager = new LikeManager();
        private CommanetManager commentManager = new CommanetManager();

        public override int Delete(Category category)
        {
            foreach (Note notes in category.Notes.ToList())
            {
                

                foreach (Liked likes in notes.Likes.ToList() )
                {
                    likeManager.Delete(likes);
                }

                foreach (Comment comment in notes.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                noteManager.Delete(notes);
            }
            return base.Delete(category);
        }

    }
}
