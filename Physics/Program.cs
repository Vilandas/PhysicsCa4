using System;
using static Physics.Enums;

namespace Physics
{
    //Paudric Smith - D00215637
    //Vilandas Morrissey - D00218436

    public class Program
    {
        private static void Main(string[] args)
        {
            new Program();
        }

        private string inKey = "\n)> ";

        public Program()
        {
            Start();
        }

        public void Demo(Properties prop)
        {
            Rk4 p = new Rk4(prop);

            Console.WriteLine("PV(0):\n" + new Vector2(prop.Position, prop.Velocity));
            while(prop.Position.Z > 0)
            {
                Vector2 pvt = p.CalculateRk4();
                if (pvt.X.IsZero() && pvt.Y.IsZero())
                {
                    Console.WriteLine("\nStatic object");
                    return;
                }
                else
                {
                    prop.Time += prop.Steps;
                    Console.WriteLine("\nPV(" + prop.Time + "):\n" + pvt);
                    p.UpdatePV(pvt);
                }
            }
            p.Data.ExportData();
            prop.Time = prop.OriginalTime;
            prop.Steps = prop.OriginalSteps;
            prop.Position = prop.OriginalPosition;
            prop.Velocity = prop.OriginalVelocity;
        }



        public void Start()
        {
            bool run = true;
            while (run)
            {
                MenuOptions menuChoice = (MenuOptions)Enum.Parse(typeof(MenuOptions), GetChoice(typeof(MenuOptions)).ToString());
                switch (menuChoice)
                {
                    case MenuOptions.Exit:
                        run = false;
                        break;
                    case MenuOptions.Example_one:
                        Demo(ExampleData.example1);
                        break;
                    case MenuOptions.Example_two:
                        Demo(ExampleData.example2);
                        break;
                    case MenuOptions.Example_three:
                        Demo(ExampleData.example3);
                        break;
                    case MenuOptions.Custom:
                        Demo(ExampleData.custom);
                        break;
                    case MenuOptions.Modify_Custom:
                        ModifyMenu();
                        break;
                }
            }
        }

        private void ModifyMenu()
        {
            bool run = true;

            while(run)
            {
                ExampleData.custom.DisplayDetails();
                ModifyOptions menuChoice = (ModifyOptions)Enum.Parse(typeof(ModifyOptions), GetChoice(typeof(ModifyOptions)).ToString());
                
                if(menuChoice != ModifyOptions.Exit)
                    Console.WriteLine("\nModifying " + Enums.GetModifyName(menuChoice) + ":");

                switch (menuChoice)
                {
                    case ModifyOptions.Exit:
                        run = false;
                        break;
                    case ModifyOptions.Modify_Gravity:
                        ExampleData.custom.Gravity = GetDouble();
                        break;
                    case ModifyOptions.Modify_Time:
                        ExampleData.custom.Time = ExampleData.custom.OriginalTime = GetDouble();
                        break;
                    case ModifyOptions.Modify_Time_Steps:
                        ExampleData.custom.Steps = ExampleData.custom.OriginalSteps = GetDouble();
                        break;
                    case ModifyOptions.Modify_Mass:
                        ExampleData.custom.Mass = GetDouble();
                        break;
                    case ModifyOptions.Modify_Dimensions:
                        ExampleData.custom.Position = GetVector();
                        break;
                    case ModifyOptions.Modify_Position:
                        ExampleData.custom.Position = ExampleData.custom.OriginalPosition = GetVector();
                        break;
                    case ModifyOptions.Modify_Velocity:
                        ExampleData.custom.Velocity = ExampleData.custom.OriginalVelocity = GetVector();
                        break;
                }
            }
        }

        /**
        * Ask user for input and check if it is in the menu choice range.
        * @return Menu choice number (int)
        */
        private object GetChoice(Type enumType)
        {
            while (true)
            {
                DisplayMenu(enumType);
                int number = GetInt();

                Array menus = Enum.GetValues(enumType);
                if (number < 0 || number >= menus.Length)
                {
                    Console.WriteLine("\nInvalid number");
                    continue;
                }
                else return menus.GetValue(number);
            }
        }

        /**
        * Display menu options
        */
        private void DisplayMenu(Type enumType)
        {
            Console.WriteLine("\nInput number to select option: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string[] names = Enums.GetNames(enumType);
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine("\t" + i + ")" + names[i]);
            }
            Console.ResetColor();
            Console.Write("Enter a number" + inKey);
        }

        /**
        * Ask user for input until they enter a valid Integer number
        * @return integer number
        */
        public int GetInt()
        {
            string input;
            do
            {
                input = Console.ReadLine();
            } while (!IsValidNumber(input, false));
            return Convert.ToInt32(input);
        }

        /**
        * Ask user for input until they enter a valid Double number
        * @return long number
        */
        private double GetDouble()
        {
            string input;
            do
            {
                input = Console.ReadLine();
            } while (!IsValidNumber(input, true));
            return Convert.ToDouble(input);
        }

        private Vector3 GetVector()
        {
            Console.WriteLine("Input X value" + inKey);
            double x = GetDouble();
            Console.WriteLine("Input Y value" + inKey);
            double y = GetDouble();
            Console.WriteLine("Input Z value" + inKey);
            double z = GetDouble();

            return new Vector3(x, y, z);
        }

        /**
        * Check to see if an input can be parsed to int or double.
        * @param input String of user inputted numbers/letters
        * @param isDouble boolean if true, check to see if the input can be parsed to Double instead of Integer.
        * @return True if the input is numeric and can be parsed. False otherwise.
        */
        private bool IsValidNumber(string input, bool isDouble)
        {
            if (input == null)
            {
                DisplayInvalidInputError();
                return false;
            }
            try
            {
                if(isDouble)
                {
                    Convert.ToDouble(input);
                }
                else Convert.ToInt32(input);
                return true;
            }
            catch (FormatException)
            {
                DisplayInvalidInputError();
                return false;
            }
            catch (OverflowException)
            {
                DisplayInvalidInputError();
                return false;
            }
        }

        private void DisplayInvalidInputError()
        {
            Console.WriteLine("\nInvalid input, numbers only");
            Console.Write("\nEnter a number" + inKey);
        }

    }
}