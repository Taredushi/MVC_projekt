using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_projekt.Models.Helpers
{
    public class ViewTools
    {
        public BookItemViewModel GetViewModel(BookItem book, ApplicationDbContext db)
        {

            BookItemViewModel bookView = new BookItemViewModel();
            bookView.ID = book.BookItemID;
            bookView.Title = book.Title;
            bookView.ISBN = book.ISBN;
            bookView.Publisher = book.Publisher;
            bookView.ReleaseDate = book.ReleaseDate;
            bookView.Descryption = book.Descryption;
            if (book.Category != null)
            {
                bookView.Category = db.Categories.Single(c => c.CategoryID == book.Category.CategoryID);
                bookView.CategoryID = book.Category.CategoryID;
            }
            bookView.Authors = db.Authors.Where(a => a.AuthorGroups.Any(g => g.BookItem.BookItemID == book.BookItemID)).ToList();
            bookView.Labels = db.Labels.Where(a => a.LabelGroups.Any(g => g.BookItem.BookItemID == book.BookItemID)).ToList();
            bookView.Amount = book.Amount;
            bookView.SelectedLabels = bookView.Labels.Select(x => x.LabelID).ToList();
            bookView.SelectedAuthors = bookView.Authors.Select(x => x.AuthorID).ToList();

            return bookView;
        }

        public void CreateBookItem(BookItemViewModel bookView, ApplicationDbContext db)
        {
            BookItem bookItem = new BookItem()
            {
                Title = bookView.Title,
                ISBN = bookView.ISBN,
                Descryption = bookView.Descryption,
                Publisher = bookView.Publisher,
                ReleaseDate = bookView.ReleaseDate,
                Category = db.Categories.Find(bookView.CategoryID),
                Amount = bookView.Amount
            };

            db.Set<BookItem>().AddOrUpdate(bookItem);
            db.SaveChanges();

            var book = db.BookItems.FirstOrDefault(x => x.ISBN == bookItem.ISBN);
            db.Categories.Find(bookView.CategoryID).BookItem.Add(book);

            foreach (var authorId in bookView.SelectedAuthors)
            {
                var author = db.Authors.Single(a => a.AuthorID == authorId);
                var ag = new AuthorGroup()
                {
                    Author = author,
                    BookItem = book
                };
                db.Set<AuthorGroup>().AddOrUpdate(ag);
            }
            db.SaveChanges();

            foreach (var labelId in bookView.SelectedLabels)
            {
                var label = db.Labels.Single(l => l.LabelID == labelId);
                var labelgroup = new LabelGroup()
                {
                    Label = label,
                    BookItem = book,
                };
                db.Set<LabelGroup>().AddOrUpdate(labelgroup);
            }
            db.SaveChanges();
        }

        public BookItem EditBookItem(BookItemViewModel bookView, ApplicationDbContext db)
        {
            var existingBook = db.BookItems.Single(x => x.BookItemID == bookView.ID);
            existingBook.Title = bookView.Title;
            existingBook.ISBN = bookView.ISBN;
            existingBook.Descryption = bookView.Descryption;
            existingBook.Publisher = bookView.Publisher;
            existingBook.Category = db.Categories.Find(bookView.CategoryID);
            existingBook.Amount = bookView.Amount;

            //Update AuthorGroup Table
            List<int> oldAuthorGroupID = db.Authors.Where(a => a.AuthorGroups.Any(g => g.BookItem.BookItemID == existingBook.BookItemID)).Select(x => x.AuthorID).ToList();
            var authorDiffAdd = bookView.SelectedAuthors.Except(oldAuthorGroupID);
            var authorDiffDel = oldAuthorGroupID.Except(bookView.SelectedAuthors);

            foreach (var arg in authorDiffDel)
            {
                AuthorGroup delete = db.AuthorGroups.Single(x => x.Author.AuthorID == arg && x.BookItem.BookItemID == bookView.ID);
                db.AuthorGroups.Remove(delete);
            }
            db.SaveChanges();
            foreach (var arg in authorDiffAdd)
            {
                AuthorGroup add = new AuthorGroup()
                {
                    Author = db.Authors.Find(arg),
                    BookItem = existingBook
                };
                db.AuthorGroups.Add(add);
            }
            db.SaveChanges();

            //Update LabelGroupTable
            List<int> oldLabelGroupID = db.Labels.Where(a => a.LabelGroups.Any(g => g.BookItem.BookItemID == existingBook.BookItemID)).Select(x => x.LabelID).ToList();
            var labelDiffAdd = bookView.SelectedLabels.Except(oldLabelGroupID);
            var labelDiffDel = oldLabelGroupID.Except(bookView.SelectedLabels);

            foreach (var arg in labelDiffDel)
            {
                LabelGroup delete = db.LabelGroups.Single(x => x.Label.LabelID == arg && x.BookItem.BookItemID == bookView.ID);
                db.LabelGroups.Remove(delete);
            }
            db.SaveChanges();
            foreach (var arg in labelDiffAdd)
            {
                LabelGroup add = new LabelGroup()
                {
                    Label = db.Labels.Find(arg),
                    BookItem = existingBook
                };
                db.LabelGroups.Add(add);
            }
            db.SaveChanges();


            return existingBook;
        }

        public IQueryable<SelectListItem> GetAuthorsFromDb(ApplicationDbContext db)
        {
            var authors = from a in db.Authors
                          select new SelectListItem
                          {
                              Value = a.AuthorID.ToString(),
                              Text = a.Surname + " " + a.Name
                          };

            return authors;
        }

        public BookItemViewModel GetBookViewModel(BookItem book, ApplicationDbContext db)
        {

            BookItemViewModel bookView = new BookItemViewModel();
            bookView.ID = book.BookItemID;
            bookView.Title = book.Title;
            bookView.ISBN = book.ISBN;
            bookView.Publisher = book.Publisher;
            bookView.ReleaseDate = book.ReleaseDate;
            bookView.Descryption = book.Descryption;
            if (book.Category != null)
            {
                bookView.Category = db.Categories.Single(c => c.CategoryID == book.Category.CategoryID);
                bookView.CategoryID = book.Category.CategoryID;
            }
            bookView.Authors =
                db.Authors.Where(a => a.AuthorGroups.Any(g => g.BookItem.BookItemID == book.BookItemID)).ToList();
            bookView.Labels =
                db.Labels.Where(a => a.LabelGroups.Any(g => g.BookItem.BookItemID == book.BookItemID)).ToList();
            bookView.Amount = book.Amount;
            bookView.SelectedLabels = bookView.Labels.Select(x => x.LabelID).ToList();
            bookView.SelectedAuthors = bookView.Authors.Select(x => x.AuthorID).ToList();
            bookView.AvailableNumber = book.Amount - db.Orders.Count(o => o.Book.BookID == book.BookItemID && o.Book.Returned);
            return bookView;
        }

        public AuthorViewModel GetAuthorViewModel(Author author, ApplicationDbContext db)
        {
            AuthorViewModel authorView = new AuthorViewModel()
            {
                ID = author.AuthorID,
                FullName = author.FullName,
                BooksNumber = db.BookItems
                    .Count(a => a.AuthorGroups.Any(x => x.Author.AuthorID == author.AuthorID))
            };

            return authorView;
        }

        public TitleViewModel GetTitleViewModel(IGrouping<string, BookItem> book, ApplicationDbContext db)
        {
            TitleViewModel titleView = new TitleViewModel()
            {
                Title = book.Key,
                BooksNumber = book.Count()
            };

            return titleView;
        }

        public LabelViewModel GetLabelViewModel(Label label, ApplicationDbContext db)
        {
            LabelViewModel labelView = new LabelViewModel()
            {
                ID = label.LabelID,
                Name = label.Name,
                BooksNumber = db.BookItems
                    .Count(a => a.LabelGroups.Any(x => x.Label.LabelID == label.LabelID))
            };

            return labelView;
        }
    }
}