using MyEvernote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.DataAccessLay.EntityFramework
{
    public class RepositoryBase
    {

        //Singleton Pattern Databasecontext isimli classımız sadece bir instance alacak (bir kere new lenecek)
        protected static DatabaseContext db;
        private static object lockSenkronize =new object();

        public RepositoryBase()
        {
            CreatContext();
        }

        public static void CreatContext()
        {
            if (db == null)
            {
                lock (lockSenkronize)
                {
                    db = new DatabaseContext();
                }

            }
        }
    }
}
