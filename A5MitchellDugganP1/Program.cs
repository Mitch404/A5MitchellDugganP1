/*  Project Name: A5MitchellDugganP1
 *  Program: Program.cs
 *
 *  Description: This file handles all menus and input gathering from the user,
 *      all management of the seating plan itself occurs in the SeatingPlan
 *      class.
 *
 *  Revision History:
 *      December 2016: Mitchell Duggan
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5MitchellDugganP1
{
    class Program
    {
        const int MINROW = 4;

        // GetMinSeatValue will retrieve a value from the user which is at
        // minimum the same as the MINROW constant
        static int GetMinSeatValue()
        {
            int num = MINROW;
            string input;
            bool valid = false;            

            do
            {
                input = Console.ReadLine();
                try
                {
                    num = Convert.ToInt32(input);
                    if (num < MINROW)
                    {
                        Console.Write("\nValue must be at least ");
                        Console.WriteLine(MINROW + ".\n");
                        Console.WriteLine("Please enter a value: ");
                        valid = false;
                    }
                    else
                    {
                        valid = true;
                    }
                }
                catch // This catch occurs when a non-number is given
                {
                    Console.WriteLine("\nPlease enter a valid number:");
                    valid = false;
                }
                
                
            } while (!valid);

            return num;

        }

        // This method will request from the user a value no larger than
        // the input supplied, or a default of 9999
        static int GetValue(int max = 9999)
        {
            int num = 1;
            string input;
            bool valid = false;

            do
            {
                input = Console.ReadLine();
                try
                {
                    num = Convert.ToInt32(input);
                    if (num > max)
                    {
                        Console.Write("\nValue must be no more than ");
                        Console.WriteLine(max + ".\n");
                        Console.WriteLine("Please enter a value: ");
                        valid = false;
                    }
                    else
                    {
                        valid = true;
                    }
                }
                catch
                {
                    Console.WriteLine("\nPlease enter a valid number:");
                    valid = false;
                }


            } while (!valid);

            return num;
        }

        static void Main(string[] args)
        {
            string input;
            int maxX = 0, maxY = 0;
            SeatingPlan plan = null;
            bool valid;
            bool cont = true;
            string firstName, lastName;
            string[] fullName;
            int newX, newY;

            // Initialization Menu loop
            do
            {
                Console.WriteLine("*********************************");
                Console.WriteLine("*          Start Menu           *");
                Console.WriteLine("* 1. Load existing seating plan *");
                Console.WriteLine("* 2. Create new seating plan    *");
                Console.WriteLine("*********************************");

                input = Console.ReadLine();

                if (input == "1") // Load existing
                {
                    // Default constructor reads from file
                    plan = new SeatingPlan();
                    valid = true; // input is valid
                }
                else if (input == "2") // Create new
                {
                    Console.WriteLine("\nPlease enter total number of rows:");
                    // GetMinSeatValue will get a value of at minimum the value
                    // of the MINROW constant
                    maxX = GetMinSeatValue(); 

                    Console.Write("\nPlease enter total number of seats ");
                    Console.WriteLine("per row.");
                    maxY = GetMinSeatValue();

                    // Construct our new seating plan
                    plan = new SeatingPlan(maxX, maxY);
                    valid = true;
                }
                else
                {
                    Console.WriteLine("\nInvalid input.");
                    valid = false;
                }
            }while (!valid); // loop until valid input received

            // If the max row and column values are zero then we read from
            // a file, this will update maxX and maxY
            if (maxX == 0 || maxY == 0)
            {
                maxX = plan.GetMaxX();
                maxY = plan.GetMaxY();
            }

            // resetting the input variable
            input = ""; 

            // Main menu loop
            do
            {   
                // We're checking input's value from the previous request
                // 3 requests alternate display
                if (input != "3")
                {
                    plan.DisplayAllSeats();
                }
                                
                Console.WriteLine("\n***************************");
                Console.WriteLine("*        Main Menu        *");
                Console.WriteLine("* 1. Add a reservation    *");
                Console.WriteLine("* 2. Remove a reservation *");
                Console.WriteLine("* 3. Display Vacant Seats *");
                Console.WriteLine("* 4. Regular Seat Display *");
                Console.WriteLine("* 5. Exit                 *");
                Console.WriteLine("***************************");
                input = Console.ReadLine();

                if (input == "1") // Add Reservation
                {
                    if (!plan.anyVacant())
                    {
                        Console.WriteLine("\nNo available seats.");
                        Console.WriteLine("Reservation attempt failed.\n");
                    }
                    else
                    {
                        Console.WriteLine("Please enter first and last name:");
                        input = Console.ReadLine();
                        fullName = input.Split(' ');

                        // This try will catch single word names entered and
                        // negative row/column values
                        try
                        {
                            // Split the full name into appropriate variables
                            firstName = fullName[0];
                            lastName = fullName[1];
                                        
                            // Get a number from user no larger than total rows
                            Console.WriteLine("Please enter a row: ");
                            newX = GetValue(maxX);

                            // Get number from user no larger than total columns
                            Console.WriteLine("Please enter a column: ");
                            newY = GetValue(maxY);

                            // Attempt to add reservation
                            plan.AddReservation(firstName, lastName, newX, newY);
                        }
                        // This will handle a negative index
                        catch (IndexOutOfRangeException)
                        {
                            if (fullName.Length != 2)
                            {
                                Console.Write("\nInvalid name. ");
                            }
                            else
                            {
                                Console.Write("\nInvalid row or column value. ");
                            }
                            Console.WriteLine("Failed to add reservation.\n");
                        }
                        catch // This catch is likely due to a single word name
                        {
                            Console.Write("\nInvalid name. Failed to add ");
                            Console.WriteLine("reservation.\n");
                        }
                    }

                    // resetting input for menu display check
                    input = "1";
                    cont = true;
                }
                else if (input == "2") // Remove Reservation
                {                  

                    do
                    {
                        Console.Write("Please enter full name or row number:");
                        Console.WriteLine();
                        input = Console.ReadLine();

                        // If the convert throws an error then we treat
                        // input as being a name.
                        // If the RemoveReservation throws an error
                        // then it will be an IndexOutOfRange exception
                        try
                        {
                            // if this line succeeds then we treat it as a row
                            newX = Convert.ToInt32(input);
                            
                            if (newX <= maxX)
                            {
                                Console.WriteLine("Please enter a column:");
                                newY = GetValue(maxY);

                                // SeatingPlan will now attempt to remove
                                // the reservation
                                plan.RemoveReservation(newX, newY);

                                valid = true;
                            }
                            else
                            {
                                Console.WriteLine("\n Invalid row number.\n");
                                valid = false;
                            }
                        }
                        // This occurs if negative index given
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine();
                            Console.Write("Invalid row or column value.\n\n");
                            valid = false;
                        }
                        catch // This means check for name
                        {
                            // This try is to catch instances where only one
                            // word or name is given
                            try
                            {
                                fullName = input.Split(' ');
                                firstName = fullName[0];
                                lastName = fullName[1];

                                // Attempt to remove reservation
                                plan.RemoveReservation(firstName, lastName);
                                valid = true;
                            }
                            catch
                            {
                                Console.Write("\nInvalid name. Failed to add ");
                                Console.WriteLine("reservation.\n");
                                valid = false;
                            }
                        }

                    } while (!valid);

                    // resetting input for menu display check
                    input = "2";            
                    cont = true;
                }
                else if (input == "3") // Display vacant seats
                {
                    plan.DisplayVacantSeats();
                }
                else if (input == "5") // Exit Case
                {
                    cont = false;
                }
                else
                {
                    // This is here because we do nothing if input == "4"
                    // which is the regular display option
                    if (input != "4")
                    {
                        Console.WriteLine("\nInvalid input.\n");
                    }                    
                }


            } while (cont); // Loops until exit case reached

        } // End Main
    }
}
