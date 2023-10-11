using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    [Serializable]
    public class GlobalDBParamObject
    {
        public string Name { get; set; }
        public Object Value { get; set; }
        public OracleDbType DbType { get; set; }
        public int? Size { get; set; }
        public Boolean isCursor { get; set; }
        public System.Data.ParameterDirection vDirection { get; set; }
        public GlobalDBParamObject() { }
        // Constructor para parametros sin tipo de dato
        public GlobalDBParamObject(String vName, Object vObject)
        {
            this.Name = vName;
            this.Value = vObject;
        }

        // Constructor para parámetros con tipos de dato especifico
        public GlobalDBParamObject(string name, object value, OracleDbType dbType)
        {
            Name = name;
            Value = value;
            DbType = dbType;
        }

        // Constructor para parámetros tipo de datos y con longitud especificada
        public GlobalDBParamObject(string name, object value, OracleDbType dbType, int size)
        {
            Name = name;
            Value = value;
            DbType = dbType;
            Size = size;
        }
        public GlobalDBParamObject(String vName, Object vObject, System.Data.ParameterDirection vDirection)
        {
            this.Name = vName;
            this.Value = vObject;
            this.vDirection = vDirection;
        }
        public GlobalDBParamObject(String vName, Object vObject, System.Data.ParameterDirection vDirection, Boolean isRefCursor)
        {
            this.Name = vName;
            this.Value = vObject;
            this.vDirection = vDirection;
            this.isCursor = isRefCursor;
        }
    }
}