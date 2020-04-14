using DHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHelper.Interface
{
    public interface IDapperCommand
    {
        long Insert<T>(object entity) where T : class, new();

        long Insert<T>(T entity) where T : class, new();

        long Insert<T>(List<object> listEntity) where T : class, new();

        long Insert<T>(List<T> listEntity) where T : class, new();

        bool Update<T>(T entity) where T : class, new();

        bool Update<T>(object entity, string condition, List<ColumInfo> updateCols) where T : class, new();

        bool Delete<T>(T entity) where T : class, new();

        bool Delete<T>(object entity, string condition) where T : class, new();

        List<T> Select<T>(string condition, string orderby, object _entity, int size = 0) where T : class, new();

        T SelectSingle<T>(string condition, string orderby, object _entity) where T : class, new();

        List<T> SelectByPage<T>(string condition, string orderby, object _entity, int startIndex, int endIndex) where T : class, new();

        T GetEntityBySql<T>(string strSql, object _entity = null);

        List<T> GetEntityListBySql<T>(string strSql, object _entity = null);

        long ExcuteBySql(string strSql, object _entity = null);
    }
}
