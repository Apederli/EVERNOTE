using Myevernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entitie;
using MyEverNote.DataAccessLay.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLay.EntityFramework
{
    public class Repository <T> : RepositoryBase , IDataAccess<T> where T : class
    {

        //private DatabaseContext db;
        private DbSet<T> _objectSet;

        public Repository()
        {
            //db = RepositoryBase.CreatContext();
            _objectSet = db.Set<T>();
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public int Insert(T obj)
        {
            if(obj is MyEntitiesBase)
            {
                MyEntitiesBase o = obj as MyEntitiesBase;
                DateTime now = DateTime.Now;
                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsurname();
            }
             _objectSet.Add(obj);
            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntitiesBase)
            {
                MyEntitiesBase o = obj as MyEntitiesBase;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.Common.GetCurrentUsurname();
            }
            return Save();
        }

        public int Delete (T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return db.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }
    }
}

//NOT 1=Burada Repository patternünü kullandım, Repository pattern tüm tablolarda yapılacak olan işlemi delete insert vb. tek bir tabloda yapmamızı sağladı.

//RepositoryBase classı Singleton pattern yaptık bir DatabaseConteks sınıfının birden fazla olmuşmasını önüne geçtik. Commetn clasıından bir yorum ürettiğimizde commet clasıı diğer claslardan veri alır mesela owner proprtisi yorumun hasibi kimdir bu başka clastan geleceği için önce o classın bir new DatabaseContext oluştuması gerekli aynı zamanda commette bir DatabaseContext oluşturucak yani new leyecek bu da birden çok databasecontext oluşacağı için hata ile karilaşılcak


//RepositoryBaseden miras aldım kod fazlalığından kurtardım