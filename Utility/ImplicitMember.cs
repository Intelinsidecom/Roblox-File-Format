using System;
using System.Linq;
using System.Reflection;

namespace RobloxFiles.Utility
{
    // This is a lazy helper class to disambiguate between FieldInfo and PropertyInfo
    internal class ImplicitMember
    {
        private const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.IgnoreCase;
        
        private readonly object member;
        private readonly string inputName;
        
        private ImplicitMember(FieldInfo field, string name) 
        { 
            member = field;
            inputName = name;
        }
        private ImplicitMember(PropertyInfo prop, string name)
        {
            member = prop;
            inputName = name;
        }

        public static ImplicitMember Get(Type type, string name)
        {
            var field = type
                .GetFields(flags)
                .Where(f => f.Name == name)
                .FirstOrDefault();

            if (field != null)
                return new ImplicitMember(field, name);

            var prop = type
                .GetProperties(flags)
                .Where(p => p.Name == name)
                .FirstOrDefault();

            if (prop != null)
                return new ImplicitMember(prop, name);

            return null;
        }

        public Type MemberType
        {
            get
            {
                if (member is PropertyInfo)
                    return ((PropertyInfo)member).PropertyType;

                if (member is FieldInfo)
                    return ((FieldInfo)member).FieldType;

                return null;
            }
        }

        public object GetValue(object obj)
        {
            object result = null;

            if (member is FieldInfo)
                result = ((FieldInfo)member).GetValue(obj);
            if (member is PropertyInfo)
                result = ((PropertyInfo)member).GetValue(obj);

            return result;
        }

        public void SetValue(object obj, object value)
        {
            if (member is FieldInfo)
            {
                ((FieldInfo)member).SetValue(obj, value);
            }
            else if (member is PropertyInfo)
            {
                ((PropertyInfo)member).SetValue(obj, value);
            }
            else
            {
                RobloxFile.LogError($"Unknown field '{inputName}' in ImplicitMember.SetValue");
            }
        }
    }
}
