using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVC_projekt.Models.View;

namespace MVC_projekt.Models.Helpers
{
    public class ViewTools
    {

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
                Number = bookView.Number
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
            existingBook.Number = bookView.Number;

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
            bookView.Number = book.Number;
            bookView.SelectedLabels = bookView.Labels.Select(x => x.LabelID).ToList();
            bookView.SelectedAuthors = bookView.Authors.Select(x => x.AuthorID).ToList();
            bookView.AvailableNumber = book.Number - db.Orders.Count(o => o.BookItemID == book.BookItemID && o.Returned);
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

        public void SaveSearch(string userId, string url, ApplicationDbContext db)
        {
            if (!db.SearchResults.Any(x => x.URL.Equals(url) && x.Account.Id.Equals(userId)))
            {
                var user = db.Users.Find(userId);

                SearchResult sr = new SearchResult()
                {
                    Account = user,
                    URL = url
                };

                db.SearchResults.Add(sr);
                db.SaveChanges();
            }

        }

        public List<CategoryViewModel> GetCategoryList_WithChildren(ApplicationDbContext db)
        {
            var parents = db.Categories.Where(x => x.Parent == null);

            List<CategoryViewModel> list = new List<CategoryViewModel>();

            foreach (var parent in parents)
            {
                CategoryViewModel cat = new CategoryViewModel();
                cat.ID = parent.CategoryID;
                cat.text = parent.Name;
                cat.nodes = GetCategory_Children(db, parent.CategoryID);
                cat.tags[0] = parent.BookItem.Count;
                cat.tags[0] += GetCategoryBooksNumber(cat.nodes);

                if (parent.BookItem.Any())
                {
                    cat.href = "CategorySearch?id=" + cat.ID;
                }

                list.Add(cat);
            }
            return list;
        }

        private List<CategoryViewModel> GetCategory_Children(ApplicationDbContext db, int id)
        {
            List<CategoryViewModel> list = new List<CategoryViewModel>();

            var parents = db.Categories.Where(x => x.Parent.CategoryID == id);

            foreach (var parent in parents)
            {
                CategoryViewModel cat = new CategoryViewModel();
                cat.ID = parent.CategoryID;
                cat.text = parent.Name;
                cat.tags[0] = parent.BookItem.Count;
                cat.nodes = GetCategory_Children(db, parent.CategoryID);
                cat.tags[0] += GetCategoryBooksNumber(cat.nodes);

                if (parent.BookItem.Any())
                {
                    cat.href = "CategorySearch?id=" + cat.ID;
                }

                list.Add(cat);
            }

            return list;
        }

        private int GetCategoryBooksNumber(List<CategoryViewModel> list)
        {
            int number = 0;

            foreach (var category in list)
            {
                number += category.tags[0];
                number += GetCategoryBooksNumber(category.nodes);
            }
            return number;
        }

    }
}