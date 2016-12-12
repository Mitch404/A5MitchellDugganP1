/*  Class: Seat
 *
 *  Description: The class for an individual seat. It contains its position,
 *      whether or not it is vacant, and a name of the person reserving it.
 *      It can free its own reservation and flip its state to reserved.
 *      It also has a default display method.
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
    class Seat
    {
        private string firstName;
        private string lastName;
        private bool vacant;
        private int x;
        private int y;

        // create a new, vacant seat
        public Seat(int newX, int newY)
        {
            vacant = true;
            x = newX;
            y = newY;
            firstName = "";
            lastName = "";
        }

        // This constructs a seat as already reserved
        public Seat (int newX, int newY, string newFirstName, 
            string newLastName)
        {
            vacant = false;
            x = newX;
            y = newY;
            firstName = newFirstName;
            lastName = newLastName;
        }

        // Default display method for the Seat class
        public void Display()
        {
            // If not vacant, display first initial and last name
            // Example: "J. Smith"
            if (!vacant) 
            {
                Console.Write(firstName[0].ToString() + ". " + lastName);
            }
            else // If vacant it will display its coordinates
            {
                Console.Write("Seat " + (x + 1) + "-" + (y + 1));
            }
        }

        // Reserves this seat with name given, will fail if already reserved
        public void Reserve(string newFirstName, string newLastName)
        {
            if (vacant == false)
            {
                Console.WriteLine("\nSeat is already reserved.\n");
            }
            else
            {
                vacant = false;
                firstName = newFirstName;
                lastName = newLastName;
            }
        }

        // Removes reservation from current Seat and resets stored names
        public void Free()
        {
            vacant = true;
            firstName = "";
            lastName = "";
        }

        // returns vacancy status
        public bool IsVacant()
        {
            return vacant;
        }

        // returns stored names as an array
        public string[] GetName()
        {
            return new string[2] { firstName, lastName };
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }


    }
}
