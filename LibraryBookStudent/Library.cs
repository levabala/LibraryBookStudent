using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBookStudent
{
    class Library
    {
        // словарь значений (id книги) - (лист экземпляров этой книги)
        // то есть каждой книги может быть несколько штук
        public Dictionary<Guid, List<Book>> books =
            new Dictionary<Guid, List<Book>>();
        public Dictionary<Guid, Student> students =
            new Dictionary<Guid, Student>();

        // словарь значений (id студента) - (список взятых книг)
        public Dictionary<Guid, List<Guid>> books2Student =
            new Dictionary<Guid, List<Guid>>();

        public string name;

        public Library(string name)
        {
            this.name = name;
        }

        public Library pushBooks(List<Book> books)
        {
            // использую LINQ - набор функций для работы с массивами/листами
            // можешь переписать в обычный for, если непонятно
            books.ForEach(book =>
            {
                if (!this.books.ContainsKey(book.id))
                    this.books[book.id] = new List<Book>();

                this.books[book.id].Add(book);
            });

            return this;
        }

        public Library registerStudent(Student student)
        {
            // записываю в список всех студентов
            students[student.id] = student;

            // проверяю, есть ли в словаре такой студент, если нет, то создаю новый лист
            books2Student[student.id] =
                books2Student.ContainsKey(student.id) ?
                books2Student[student.id] :
                new List<Guid>();

            return this;
        }

        // функция для фиксирования взятия книги 
        // возращает значение из BookTakeResult
        public BookTakeResult registerBookTake(Student student, Guid bookId)
        {
            if (!books2Student.ContainsKey(student.id))
                return BookTakeResult.StudentIsNotRegistered;

            if (!books.ContainsKey(bookId))
                return BookTakeResult.NoSuchBook;

            if (books[bookId].Count == 0)
                return BookTakeResult.OutOfStack;

            // берём книгу
            Book book = books[bookId][0];

            // помечаем, кто взял книгу, внутри книги
            book.useHistory.Add(student.id);

            // записываем, кто взял книгу
            books2Student[student.id].Add(bookId);

            // удаляем из списка доступных
            books[bookId].RemoveAt(0);

            // отдаём студенту
            student.books.Add(book);

            // говорим, что всё ок
            return BookTakeResult.Success;
        }

        // создаём несколько (1) перезгрузок метода, чтобы можно было брать не только по id
        // а и по названию, автору и т.п.
        public BookTakeResult registerBookTake(
            Student student,
            string bookName,
            string author = null
            )
        {
            if (!books2Student.ContainsKey(student.id))
                return BookTakeResult.StudentIsNotRegistered;

            try
            {
                // ищем книгу по названию (или и по автору)
                // books.Value - список всех книг в библиотеке
                // методом First мы ищем первую книгу, удовлетворяющую условию, что
                // прописан внутри First.
                // через arg => {} я определяю функцию условия 
                List<Book> bookItems = books.Values.First(bb =>
                {
                    if (bb.Count == 0)
                        return false;

                    Book b = bb.First();
                    return b.name == bookName && (author == null || b.author == author);
                });

                if (bookItems.Count == 0)
                    return BookTakeResult.OutOfStack;

                // если всё окс
                Book book = bookItems.First();

                // вызываем оснувную перегрузку метода, которая берёт книгу по id 
                return registerBookTake(student, book.id);

            }
            catch (Exception)
            {
                return BookTakeResult.NoSuchBook;
            }
        }

        public void printState()
        {
            Console.WriteLine(string.Format("Library \"{0}\"", name));
            Console.WriteLine();

            List<string> booksInfo = new List<string>();
            foreach (List<Book> bookItems in books.Values)
                booksInfo.Add(string.Format("Book \"{0}\" by {1} ({2} items)", bookItems.First().name, bookItems.First().author, bookItems.Count));

            // складываем информацию по каждой из книжек в одну большую колбасы
            string booksInfoString = string.Join("\n", booksInfo);
            Console.WriteLine(booksInfoString);
            Console.WriteLine();

            List<string> studentsInfo = new List<string>();
            foreach (Student student in students.Values)
                studentsInfo.Add(string.Format("Student \"{0}\"", student.name));

            string studentsInfoString = string.Join("\n", studentsInfo);
            Console.WriteLine(studentsInfoString);
            Console.WriteLine();
        }
    }

    public enum BookTakeResult
    {
        StudentIsNotRegistered,
        NoSuchBook,
        OutOfStack,
        Success
    }
}
