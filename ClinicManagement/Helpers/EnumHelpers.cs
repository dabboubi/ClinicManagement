using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ClinicManagement.Core.Helpers
{
    public static class EnumHelpers
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute =
                    (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return value.ToString();
        }

    }
}
