using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    [Serializable]
    public class GlobalDBParamObjectList
    {
        GlobalDBParamObject[] vParams = new GlobalDBParamObject[100];
        int vItems = 0;

        public GlobalDBParamObjectList()
        {
            for (int i = 0; i < 100; i++)
            {
                vParams[i] = new GlobalDBParamObject();
            }
        }

        public void Add(GlobalDBParamObject vParam)
        {
            vItems++;
            vParams[vItems - 1] = vParam;
        }

        public GlobalDBParamObject get(int index)
        {
            return vParams[index];
        }

        public int Count()
        {
            return vItems;
        }
    }
}