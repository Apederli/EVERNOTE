using MyEvernote.BusinessLay;
using MyEvernote.Entitie;
using MyEvernote.WebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MyEvernote.WebApp.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("categories-cache");

            if (result == null)
            {
                CategoryManager categoryManager = new CategoryManager();
                result = categoryManager.List();
                WebCache.Set("categories-cache", result, 20, true);
            }

            return result;
        }

        public static void RemoveCategoriesFromCache()
        {
            Remove("categories-cache");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}