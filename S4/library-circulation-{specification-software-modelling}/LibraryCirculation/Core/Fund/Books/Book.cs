using System.Collections.Generic;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Fund.Books
{
    public enum BookFormat
    {
        Small,
        Medium,
        Large,
        EBook,
        AudioBook
    }

    public class Book : IKey, ISerializable
    {
        public Book() : this("", "", "", 0, "", 0, new List<int>(), "", BookFormat.Small, false)
        {
        }

        public Book(string isbn, string udk, string title, int year, string language, int publisherId,
            List<int> authorIds, string description, BookFormat format, bool hardcover)
        {
            Isbn = isbn;
            Udk = udk;
            Title = title;
            Year = year;
            Language = language;
            PublisherId = publisherId;
            AuthorIds = authorIds;
            Description = description;
            Format = format;
            Hardcover = hardcover;
        }

        public string Isbn { get; set; }
        public string Udk { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Language { get; set; }
        public int PublisherId { get; set; }
        public List<int> AuthorIds { get; set; }
        public string Description { get; set; }
        public BookFormat Format { get; set; }
        public bool Hardcover { get; set; }

        public object GetKey()
        {
            return Isbn;
        }

        public void SetKey(object key)
        {
            Isbn = (string)key;
        }

        public IReadOnlyList<string> Serialize()
        {
            return new[]
            {
                Isbn, Udk, Title, Year.ToString(), Language,
                PublisherId.ToString(), SerialUtil.Join(AuthorIds),
                Description, Format.ToString(), Hardcover.ToString()
            };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Isbn = values[0];
            Udk = values[1];
            Title = values[2];
            Year = int.Parse(values[3]);
            Language = values[4];
            PublisherId = int.Parse(values[5]);
            AuthorIds = SerialUtil.Split<int>(values[6]);
            Description = values[7];
            Format = SerialUtil.Parse<BookFormat>(values[8]);
            Hardcover = bool.Parse(values[9]);
        }
    }
}