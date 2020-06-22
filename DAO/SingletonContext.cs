using MePague.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MePague.DAO
{
    public class SingletonContext
    {
        private SingletonContext() { }
        private static Context ctx;
        public static Context GetInstance()
        {
            if (ctx == null)
            {
                ctx = new Context();
            }
            return ctx;
        }
    }
}