using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Contract.Dto.References;

public static class Roles
{
    public static string Administrator = "administrator";
    public static string Аccountant = "accountant";
    public static string Boss = "boss";
    public static string Developer = "developer";
    public static string Manager = "manager";
    public static string Popug = "popug";
    public static IEnumerable<string> All => typeof(Roles).GetFields(BindingFlags.Static | BindingFlags.Public).Select(field => field.GetValue(field)).Cast<string>();
}