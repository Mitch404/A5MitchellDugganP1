/*  Class Name: SeatingPlan
 *
 *  Description: SeatingPlan handles the management of a group of seats.
 *      It maintains a two dimensional array of seats. When given a name and
 *      pair of coordinates, it can add a reservation. When removing it can
 *      remove from a coordinate, or search for name.
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
using System.IO;

namespace A5MitchellDugganP1
{
    class SeatingPlan
    {
        private int totalVacant;
        private int maxX;
        private int maxY;
        private Seat[][] seatingPlan;
        private const string FILENAME = "seatingPlan.csv";
 


        public SeatingPlan() // Building SeatingPlan from existing File
        {
            int[] size;

            // Attempt to access file to build itself
            try
            {
                size = GetPlanSize();
                maxX = size[0];
                maxY = size[1];

                CreateSeats();

                ReadState();
            }
            // File not found, will make a new seating plan of default size
            catch (FileNotFoundException) 
            {
                Console.Write("File not found, creating plan of default ");
                Console.WriteLine("4x4 size, all entries vacant.\n");

                maxX = 4;
                maxY = 4;
                totalVacant = 16;
                CreateSeats();
            }
            // Some other exception, will make a new default size seating plan
            catch (Exception)
            {
                Console.Write("Something went wrong, creating plan of ");
                Console.WriteLine("default 4x4 size, all entries vacant.\n");

                maxX = 4;
                maxY = 4;
                totalVacant = 16;
                CreateSeats();
            }

         
        }

        // Our constructor for a new, custom seating plan
        // newX specifies our number of rows, newY specifies our columns
        public SeatingPlan(int newX, int newY)
        {
            maxX = newX;
            maxY = newY;
            totalVacant = maxX * maxY;

            CreateSeats();
            WriteState();
        }

        // The menu uses these to get maximum dimensions
        public int GetMaxX()
        {
            return maxX;
        }

        public int GetMaxY()
        {
            return maxY;
        }

        // This builds our two dimensional array of seats
        // It constructs seats for each element with their position
        // as their x and y coordinate values
        private void CreateSeats()
        {
            seatingPlan = new Seat[maxX][];

            // give each row an array of seats of length maxY
            for (int i = 0; i < maxX; i++)
            {
                seatingPlan[i] = new Seat[maxY];
            }

            // Assign each element a reference to a seat
            for (int j = 0; j < maxX; j++)
            {
                for (int i = 0; i < maxY; i++)
                {
                    seatingPlan[j][i] = new Seat(j, i);
                }
            }
        }

        // This returns a bool for whether or not we have vacancies
        public bool anyVacant()
        {
            return (totalVacant > 0);
        }

        // This creates a dividing line of *'s the same width as the
        // default display of all seats.
        // As names change the width of individual entries, it may not match
        private void DividingLine()
        {
            const string STARS = "***********";

            Console.Write("*");
            for (int i = 0; i < maxY; i++)
            {
                Console.Write(STARS);
            }
            Console.WriteLine();
        }

        // Default display of seats, instructs each seat to display
        public void DisplayAllSeats()
        {
            DividingLine();

            foreach(Seat[] row in seatingPlan)
            {
                Console.Write("* ");
                foreach(Seat table in row)
                {
                    table.Display();
                    Console.Write(" * ");
                }
                Console.WriteLine("");
            }

            DividingLine();
        }

        // This displays only vacant seats, reserved seats are blank
        public void DisplayVacantSeats()
        {
            const string BLANK = "        ";

            DividingLine();

            foreach (Seat[] row in seatingPlan)
            {
                Console.Write("* ");
                foreach (Seat table in row)
                {
                    if (table.IsVacant())
                    {
                        table.Display();
                    }
                    else
                    {
                        Console.Write(BLANK);
                    }

                    Console.Write(" * ");
                }
                Console.WriteLine("");                
            }

            DividingLine();
        }

        // This will attempt to add a reservation at the specified coordinate
        // and will pass the given names to the seat.
        public void AddReservation(string firstName, string lastName,
            int x, int y)
        {
            bool success = false;

            // Checks if indices are not greater than bounds
            if (x <= maxX & y <= maxY)
            {
                // User input is assumed to be +1 than indices, which start at
                // zero
                if (seatingPlan[x - 1][y - 1].IsVacant())
                {
                    seatingPlan[x - 1][y - 1].Reserve(firstName, lastName);
                    Console.WriteLine("\nReservation Successfully added.\n");
                    success = true;

                    totalVacant -= 1;
                    WriteState();
                }
            }

            if (!success) // Checks for general failure case.
            {
                Console.WriteLine("Reservation attempt failed.\n");
            }
        }

        // This will search for reservations to remove by names passed to it.
        // It will ask to remove any individual match found.
        public void RemoveReservation(string firstName, string lastName)
        {
            bool success = false;
            string[] tableNames;
            string input;
            bool cont = false;
            Seat table;

            for (int j = 0; j < maxX; j++)
            {

                for (int i = 0; i < maxY; i++)
                {
                   table = seatingPlan[j][i];

                   if (!(table.IsVacant()))
                    {
                        // We are checking if the supplied name matches either
                        // the format "John Smith" or "J. Smith"
                        tableNames = table.GetName();
                        if ((firstName == tableNames[0] && 
                            lastName == tableNames[1]) ||
                            (firstName == (tableNames[0][0].ToString() + ".") &&
                            lastName == tableNames[1]))
                        {
                            // success flag indicates we've encountered at
                            // least one match.
                            success = true;
                            cont = false;

                            // Request user input for whether we should remove
                            // this particular match.
                            do
                            {
                                Console.Write("\nMatch found at " + (j + 1));
                                Console.Write("-" + (i + 1));
                                Console.Write(". Remove? (Y/N): ");
                                input = Console.ReadLine();

                                if (input.ToUpper() == "Y"
                                    || input.ToUpper() == "YES")
                                {
                                    table.Free();
                                    Console.Write("\nReservation successfully");
                                    Console.WriteLine(" removed.");
                                    cont = true;
                                    totalVacant += 1;
                                }
                                else if (input.ToUpper() == "N"
                                    || input.ToUpper() == "NO")
                                {
                                    Console.WriteLine("\nSkipping match.");
                                    cont = true;
                                }
                                else
                                {
                                    cont = false;
                                    Console.Write("\nInvalid input, please ");
                                    Console.WriteLine("enter 'Y' or 'N'\n");
                                }
                                
                            } while (!cont);

                            // Update file
                            WriteState();
                        } // End If - checking for name match
                    } // End If - checking for vacancy
                } // End For               
            } // End For

            if (!success)
            {
                Console.WriteLine("Reservation not found.");
            }
            else
            {
                Console.WriteLine("No more matches.");
            }

        }

        // This RemoveReservation checks a coordinate, if the seat isn't vacant
        // then it will remove the reservation.
        // The x and y values are assumed to be +1 from the actual index value
        // due to user input.
        public void RemoveReservation(int x, int y)
        {
            if (seatingPlan[x - 1][y - 1].IsVacant())
            {
                Console.WriteLine("\nReservation not found.\n");
            }
            else // seat is reserved
            {
                seatingPlan[x - 1][y - 1].Free();
                Console.WriteLine("\nReservation successfully removed.\n");

                totalVacant += 1;
                WriteState();
            }
        }

        // WriteState dumps the vacancy and name values of all seats
        // to an external file, specified by the class constant FILENAME
        // It will write individual seats as one of the following:
        //      "TRUE, "
        //      "FALSE,John Doe"
        // Sample 2x2 file:
        //      TRUE, ,FALSE,Jane Doe
        //      FALSE,John Smith,True, 
        private void WriteState()
        {
            StreamWriter writer;
            string[] names;
            bool firstInRow = true;

            try
            {
                writer = new StreamWriter(FILENAME);                

                foreach (Seat[] row in seatingPlan)
                {
                    firstInRow = true;
                    foreach (Seat table in row)
                    {
                        if (!firstInRow)
                        {
                            writer.Write(",");
                        }
                        names = table.GetName();
                        writer.Write(table.IsVacant() + ",");
                        writer.Write(names[0] + " " + names[1]);
                        firstInRow = false;
                    }
                    writer.WriteLine("");
                }

                writer.Close();
            }
            catch // If we fail to write state, complain and continue
            {
                Console.WriteLine("\nWarning: Failed to save state.\n");
            }
        }

        // This should only be run when seatingPlan[][] is
        // entirely vacant
        // It reads based on the specification used by WriteState()
        // It additionally requires external error catching by whatever
        // called it.
        private void ReadState()
        {
            string[] names;
            string[] values;
            string line;
            int rowCount, table;
            StreamReader reader = new StreamReader(FILENAME);

            rowCount = 0;
            table = 0;

            foreach (Seat[] row in seatingPlan)
            {
                line = reader.ReadLine();
                values = line.Split(',');

                // Values per table are in pairs
                // so we will iterate 0, 2, 4,...,etc.
                for (int i = 0; i < values.Length; i += 2)
                {
                    if (!(Convert.ToBoolean(values[i])))
                    {
                        names = values[i + 1].Split(' ');
                        seatingPlan[rowCount][i / 2].Reserve(names[0], names[1]);
                        table++;
                    }                    
                    
                }

                rowCount++;    
            }

            totalVacant = (maxX * maxY) - table;

            reader.Close();
        }

        // This requires external error catching, and requires a file with
        // at minimum one line
        // It parses the same way as ReadState(), but is looking at total
        // number of values and lines to determine how big our seating plan is
        // as part of its construction process.
        private int[] GetPlanSize()
        {
            string[] values;
            string line;
            int rowCount = 0;
            int columns = 0;

            StreamReader reader = new StreamReader(FILENAME);

            line = reader.ReadLine();
            rowCount++;
            values = line.Split(',');

            columns = (values.Length / 2);

            while (reader.EndOfStream == false)
            {
                line = reader.ReadLine();
                rowCount++;
            }

            reader.Close();
            return new int[] { rowCount, columns };
        }
    }
}
