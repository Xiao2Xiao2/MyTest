using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Common;
using Model;
using DHelper.Dapper;
namespace MyTest.Extensions
{
    public class UserPermisstionsOperate
    {
        private string GUID;
        public UserPermisstionsOperate(string userid)
        {
            GUID = userid;
        }

        public Sys_UserAccount GetAdmin
        {
            get
            {
                Sys_UserAccount admin = Cache.ReadCache(GUID.ToString() + "-Admin") as Sys_UserAccount;
                if (admin == null) admin = new Sys_UserAccount();
                return admin;
            }
        }

        public void StoragePermissions()
        {
            #region 用户缓存
            //获取用户 Model
            Sys_UserAccount admin = DapperCommand.SelectSingle<Sys_UserAccount>(new { GUID }, " GUID=@GUID ");
            Cache.AddCache(GUID.ToString() + "-Admin", admin, SystemExtends.CacheExpiredTime);
            #endregion 
            if (GetAdmin.IsAdmin==0)
            {
                #region 角色缓存
                //获取用户角色集合
                List<Sys_UserRole> listUserRole = DapperCommand.Select<Sys_UserRole>(new { UserAccId = GUID }, " UserAccId=@UserAccId ").ToList();
                //获取角色OID集合
                List<string> listRoleOID = new List<string>();
                listUserRole.ForEach(x =>
                {
                    if (x.RoleID != null && x.RoleID != "")
                    {
                        string item = "'" + x.RoleID + "'";
                        if (!listRoleOID.Contains(item))
                            listRoleOID.Add(item);
                    }
                });

                //获取角色集合 保存至 缓存
                List<Sys_Role> listRoles = DapperCommand.Select<Sys_Role>(new { GUID= listRoleOID.ToArray() }, " GUID in ({0}) and Deleted = 0 ").ToList();
                Cache.AddCache(GUID.ToString() + "-Roles", listRoles, SystemExtends.CacheExpiredTime);
                #endregion

                #region 权限缓存
                //获取角色权限集合
                List<Sys_RoleRight> listRoleRight = DapperCommand.Select<Sys_RoleRight>(new { RoleID = listRoleOID.ToArray() }, " RoleID in ({0}) Deleted = 0 ").ToList(); 

                //获取权限OID集合
                List<string> listRightOID = new List<string>();
                listRoleRight.ForEach(x =>
                {
                    if (x.RightID != null && x.RightID != "")
                    {
                        string item = "'" + x.RightID + "'";
                        if (!listRightOID.Contains(item))
                            listRightOID.Add(item);
                    }
                });
                //获取权限集合 保存至缓存
                List<Sys_Right> listRights = DapperCommand.Select<Sys_Right>(new { GUID = listRightOID.ToArray() }, " GUID in ({0}) Deleted = 0 ").ToList();
                Cache.AddCache(GUID.ToString() + "-Rights", listRights, SystemExtends.CacheExpiredTime);
                #endregion

                #region 模块缓存
                //获取模块OID
                List<string> listModulesOID = new List<string>();
                listRights.ForEach(x =>
                {
                    if (x.ForModuleID != null && x.ForModuleID != "")
                    {
                        string item = "'" + x.ForModuleID + "'";
                        if (!listModulesOID.Contains(item))
                            listModulesOID.Add(item);
                    }
                });
                //获取模块集合 保存至缓存
                //if (listModulesOID.Count == 0)
                //{
                //    listModulesOID.Add("'1'");
                //}
                List<Sys_Module> listModule = DapperCommand.Select<Sys_Module>(new { GUID = listModulesOID.ToArray() }, " GUID in ({0}) Deleted = 0 ").ToList(); 
                Cache.AddCache(GUID.ToString() + "-Module", listModule, SystemExtends.CacheExpiredTime);
                #endregion
            }
        
            //}
            //catch (Exception)
            //{
            //    return;
            //}
          
           
        }


        public bool HasRightCode(string rightCode)
        {
            if (GetAdmin.IsAdmin == 1) return true;
            List<Sys_Right> listRights = Cache.ReadCache(GUID.ToString() + "-Rights") as List<Sys_Right>;
            if (listRights == null || listRights.Count == 0) return false;
            return listRights.Exists(x => x.RiCode == rightCode);
        }




    }
}
