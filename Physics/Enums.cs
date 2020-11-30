using System;

namespace Physics
{
    public class Enums
    {
        public static string[] GetNames(Type enumType)
        {
            string[] names = Enum.GetNames(enumType);

            for(int i = 0; i < names.Length; i++)
            {
                names[i] = names[i].Replace("_", " ");
            }
            return names;
        }

        public static string GetModifyName(ModifyOptions option)
        {
            string name = Enum.GetName(typeof(ModifyOptions), option);
            return name.Substring(name.IndexOf("_") + 1);
        }

        public enum MenuOptions
        {
            Exit,
            Example_one,
            Example_two,
            Example_three,
            Custom,
            Modify_Custom
        }

        public enum ModifyOptions
        {
            Exit,

            Modify_Gravity,
            Modify_Time, 
            Modify_Time_Steps,

            Modify_Mass,
            Modify_Dimensions,
            Modify_Position,
            Modify_Velocity
        }
    }
}
