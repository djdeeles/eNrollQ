using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web; 
using Enroll.Managers;
using eNroll.App_Data; 

public class RoleControl
{
    public static Entities _entities = new Entities();
    public static bool YetkiAlaniKontrol(string email, int yetkiAlaniId)
    {
        bool durum = false;
        var user = _entities.Users.First(p => p.EMail == email);
        var krList = _entities.UserRole.Where(p => p.UserId == user.Id).ToList();
        foreach (var userRole in krList)
        {
            var role = _entities.Roles.First(p => p.Id == userRole.RoleId);
            var ryaList = _entities.RoleAuthAreas.Where(
                p => p.RoleId == userRole.RoleId && p.AuthAreaId == yetkiAlaniId).ToList();
            if (ryaList.Count != 0 && role.State)
            {
                durum = true;
            }
        }
        return durum;
    }

    // aktif kullanıcının yetki alanı Id leri
    public static List<int> UserActiveAuthAreaIds()
    {
        var list = new List<int>();
        try
        {
            var user = _entities.Users.FirstOrDefault(p => p.EMail == HttpContext.Current.User.Identity.Name);
            var userRole = _entities.UserRole.FirstOrDefault(p => p.UserId == user.Id);
            if (userRole != null)
            {
                int rolId = userRole.RoleId;
                var roleAuthAreas = _entities.RoleAuthAreas.Where(p => p.RoleId == rolId).ToList();
                if (roleAuthAreas.Count > 0)
                {
                    foreach (var roleAuthArea in roleAuthAreas)
                    {
                        list.Add(roleAuthArea.AuthAreaId);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception, "RoleControl:UserActiveAuthAreaIds()");
        }

        return list;
    }

    // kullanıcı yetki alanı Id leri
    public static List<int> UserActiveAuthAreaIds(int userId)
    {
        var list = new List<int>();
        try
        {
            var userRole = _entities.UserRole.FirstOrDefault(p => p.UserId == userId);
            if (userRole != null)
            {
                int rolId = userRole.RoleId;
                list = _entities.RoleAuthAreas.Where(p => p.RoleId == rolId).Select(p => p.AuthAreaId).ToList();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception, "RoleControl:UserActiveAuthAreaIds(int userId)");
        }
        return list;
    }

    // aktif kullanıcının rol yetki alanı Id leri
    public static List<int> RoleActiveAuthAreaIds()
    {
        var list = new List<int>();
        try
        {
            var user = _entities.Users.FirstOrDefault(p => p.EMail == HttpContext.Current.User.Identity.Name);
            var role = _entities.UserRole.FirstOrDefault(p => p.UserId == user.Id);
            if (role != null)
            {
                int rolId = role.RoleId;
                list = _entities.RoleAuthAreas.Where(p => p.RoleId == rolId).Select(p => p.AuthAreaId).ToList();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception, "RoleControl:RoleActiveAuthAreaIds()");
        }

        return list;
    }

    // rol yetki alanı Id leri
    public static List<int> RoleActiveAuthAreaIds(int roleId)
    {
        var list = new List<int>();
        try
        {
            list = _entities.RoleAuthAreas.Where(p => p.RoleId == roleId).Select(p => p.AuthAreaId).ToList();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception, "RoleControl:RoleActiveAuthAreaIds()");
        }

        return list;
    }
     
}