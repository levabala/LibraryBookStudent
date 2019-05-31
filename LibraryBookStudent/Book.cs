using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBookStudent
{
    class Book
    {
        public Guid id;
        public string name, author;
        public DateTime publishDate;

        public List<Guid> useHistory = new List<Guid>();

        public Book(string name, string author, DateTime publishDate)
        {
            this.name = name;
            this.author = author;
            this.publishDate = publishDate;

            // создаём уникальный хэш для книжки
            MD5 mD5 = MD5.Create();

            // строчка, которую будем хэшировать
            string s = name + author;

            // считаем MD5 хэш из байтов строки s 
            byte[] data = mD5.ComputeHash(Encoding.Default.GetBytes(s));

            // создаём id из хэша
            id = new Guid(data);

            // P.S.
            // мы могли бы создавать id рандомно (как для студента)
            // но тогда бы могло бы случиться так, чтобы было бы 2 одинаковые книжки
        }

        public void registerUsing(Student student)
        {
            useHistory.Add(student.id);
        }
    }
}
