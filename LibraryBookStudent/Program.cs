using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBookStudent
{
    class Program
    {
        static void Main(string[] args)
        {
            // создадим пару библиотек
            Library libraryStockholm = new Library("Stockholm's Library");
            Library libraryVienna = new Library("Vienna's Library");

            // список библиотек на будущее
            Library[] libraries = new Library[]
            {
                libraryStockholm,
                libraryVienna
            };

            // и немного книжек
            string[] names = new string[]
            {
                "Сказочная борьба",
                "Моё основное наказание",
                "Прекрасный спор",
                "Слёзы моей бесконечности",
                "Курсы похищенных мерзавцев"
            };

            string[] authors = new string[]
            {
                "Кубрик К.К.",
                "Тарантино М.М.",
                "Свидригайло У.М.",
                "Тимошенко Е.Е"
            };

            Random rnd = new Random();

            // создаю пустой массив из 10 элементов
            // методом Select для каждого пустого элемента создаю Book
            // потом перевожу в лист с помощью ToList, т.к.
            // Select возвращает не лист
            List<Book> books = new Book[10].Select(_ =>            
                new Book(
                    names[rnd.Next(names.Length - 1)], 
                    authors[rnd.Next(authors.Length - 1)],
                    new DateTime(
                        rnd.Next(1500, 2000),
                        rnd.Next(1, 11),
                        rnd.Next(1, 28)
                        )
                    )
            ).ToList();

            // добавляю книжки в библиотеки (дважды)
            foreach (Library library in libraries)            
                library.pushBooks(books);

            // пара студентов
            Student studentMark = new Student("Mark");
            Student studentEvil = new Student("Evil");

            // списочек
            Student[] students = new Student[]
            {
                studentMark,
                studentEvil
            };

            // регистрируем студентиков
            libraryStockholm
                .registerStudent(studentMark)
                .registerStudent(studentEvil);

            // а тут только одного
            libraryVienna
                .registerStudent(studentMark);

            // выведем состояние в консоль
            foreach (Library library in libraries)
            {
                Console.WriteLine("---");
                library.printState();
            }

            Console.WriteLine("\n----- Setup finished -----\n");

            // попробуем взять первую книжку 
            studentMark.takeBook(libraryStockholm, books[rnd.Next(books.Count)].name);

            // ну и вторую (должно дать StudentNotRegistered)
            studentEvil.takeBook(libraryVienna, books[rnd.Next(books.Count)].name);

            // печатем, сколько у кого книжек
            foreach (Student student in students)
                Console.WriteLine(
                    string.Format(
                        "{0}'s books count: {1}", student.name, student.books.Count
                        )
                    );
            Console.WriteLine();

            // wait for extiting input
            Console.ReadKey();
        }

        static private IEnumerable<T> shuffleArray<T>(IEnumerable<T> arr)
        {
            Random rnd = new Random();
            IEnumerable<T> shuffledArr = arr.OrderBy(x => rnd.Next()).ToArray();

            return shuffledArr;
        }
    }
}
