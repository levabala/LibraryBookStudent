using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBookStudent
{
    class Student
    {
        // Guid - класс для создания уникальных идентификаторов
        public Guid id = Guid.NewGuid();
        public List<Book> books = new List<Book>();
        public string name;

        public Student(string name)
        {
            this.name = name;
        }

        public void registerAtLibrary(Library library)
        {
            library.registerStudent(this);
        }

        public BookTakeResult takeBook(Library library, string name, string author = null)
        {
            string s =
                string.Format(
                    "Student \"{0}\" is trying to take book \"{1}\" {2}",
                    this.name,
                    name,
                    author != null ? string.Format("written by {0}", author) : ""
                );
            Console.WriteLine(s);

            BookTakeResult result = library.registerBookTake(this, name, author);

            switch (result)
            {
                case BookTakeResult.NoSuchBook:
                    Console.WriteLine("No such book in this library");
                    break;
                case BookTakeResult.OutOfStack:
                    Console.WriteLine("Out of stack in this library");
                    break;
                case BookTakeResult.StudentIsNotRegistered:
                    Console.WriteLine("This student is not registered in this library");
                    break;
                case BookTakeResult.Success:
                    Console.WriteLine("Successfully taken");
                    break;
            }

            Console.WriteLine();

            return result;
        }
    }
}
