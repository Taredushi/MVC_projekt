﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVC_projekt.Models.Classes;
using MVC_projekt.Models.View;

namespace MVC_projekt.Models.Helpers
{
    public class ViewTools
    {

        public void CreateBookItem(BookItemViewModel bookView, ApplicationDbContext db, HttpPostedFileBase cover, HttpPostedFileBase tableOfContents)
        {

            BookItem bookItem = new BookItem()
            {
                Title = bookView.Title,
                ISBN = bookView.ISBN,
                Descryption = bookView.Descryption,
                Publisher = bookView.Publisher,
                ReleaseDate = bookView.ReleaseDate,
                Category = db.Categories.Find(bookView.CategoryID),
                Number = bookView.Number,
                AddDate = DateTime.Now
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

            if (cover != null)
            {
                AddAttachments_displayable(db, bookItem.BookItemID, cover, FileType.Cover);
            }
            if (tableOfContents != null)
            {
                AddAttachments_displayable(db, bookItem.BookItemID, tableOfContents, FileType.TableOfContents);
            }

            foreach (var file in bookView.FileList)
            {
                AddAttachments(db, bookItem.BookItemID, file, FileType.Attachment);
            }

        }

        public BookItem EditBookItem(BookEditViewModel bookView, ApplicationDbContext db, HttpPostedFileBase cover, HttpPostedFileBase tableOfContents)
        {
            var existingBook = db.BookItems.Single(x => x.BookItemID == bookView.BookItemViewModel.ID);
            existingBook.Title = bookView.BookItemViewModel.Title;
            existingBook.ISBN = bookView.BookItemViewModel.ISBN;
            existingBook.Descryption = bookView.BookItemViewModel.Descryption;
            existingBook.Publisher = bookView.BookItemViewModel.Publisher;
            existingBook.Category = db.Categories.Find(bookView.BookItemViewModel.CategoryID);
            existingBook.Number = bookView.BookItemViewModel.Number;

            //Update AuthorGroup Table
            List<int> oldAuthorGroupID = db.Authors.Where(a => a.AuthorGroups.Any(g => g.BookItem.BookItemID == existingBook.BookItemID)).Select(x => x.AuthorID).ToList();
            var authorDiffAdd = bookView.BookItemViewModel.SelectedAuthors.Except(oldAuthorGroupID);
            var authorDiffDel = oldAuthorGroupID.Except(bookView.BookItemViewModel.SelectedAuthors);

            foreach (var arg in authorDiffDel)
            {
                AuthorGroup delete = db.AuthorGroups.Single(x => x.Author.AuthorID == arg && x.BookItem.BookItemID == bookView.BookItemViewModel.ID);
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
            var labelDiffAdd = bookView.BookItemViewModel.SelectedLabels.Except(oldLabelGroupID);
            var labelDiffDel = oldLabelGroupID.Except(bookView.BookItemViewModel.SelectedLabels);

            foreach (var arg in labelDiffDel)
            {
                LabelGroup delete = db.LabelGroups.Single(x => x.Label.LabelID == arg && x.BookItem.BookItemID == bookView.BookItemViewModel.ID);
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

            if (cover != null)
            {
                DeleteCover_Table(db, bookView.BookItemViewModel.ID, FileType.Cover);
                AddAttachments_displayable(db, bookView.BookItemViewModel.ID, cover, FileType.Cover);
            }
            if (tableOfContents != null)
            {
                DeleteCover_Table(db, bookView.BookItemViewModel.ID, FileType.TableOfContents);
                AddAttachments_displayable(db, bookView.BookItemViewModel.ID, tableOfContents, FileType.TableOfContents);
            }
            if (bookView.Cover == null && cover == null)
            {
                DeleteCover_Table(db, bookView.BookItemViewModel.ID, FileType.Cover);
            }
            if (bookView.Table == null && tableOfContents == null)
            {
                DeleteCover_Table(db, bookView.BookItemViewModel.ID, FileType.TableOfContents);
            }

            DeleteOldFile(db, bookView.BookItemViewModel.ID, bookView.OldFiles);

            if (bookView.BookItemViewModel.FileList != null)
            {
                foreach (var file in bookView.BookItemViewModel.FileList)
                {
                    AddAttachments(db, bookView.BookItemViewModel.ID, file, FileType.Attachment);
                }

            }

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
            var parents = db.Categories.Where(x => x.ParentID == 0);

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

            var parents = db.Categories.Where(x => x.ParentID == id);

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

        private void AddAttachments_displayable(ApplicationDbContext db, int bookItemID, HttpPostedFileBase file, FileType type)
        {
            string directoryPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload"), bookItemID.ToString(), "Cover");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = file.FileName.Remove(file.FileName.LastIndexOf('.')) + "_" + bookItemID +
                               Path.GetExtension(file.FileName);

            var filePath = Path.Combine(directoryPath, fileName);

            Attachment at = new Attachment()
            {
                FileType = type,
                FileName = fileName,
                BookItemID = bookItemID,
                Source = "~/Upload/" + bookItemID + "/Cover/" + fileName
            };

            file.SaveAs(filePath);
            db.Attachments.Add(at);
            db.SaveChanges();

        }

        private void AddAttachments(ApplicationDbContext db, int bookItemID, AttachmentFile file, FileType type)
        {
            string directoryPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload"), bookItemID.ToString());
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = file.File.FileName.Remove(file.File.FileName.LastIndexOf('.')) + "_" + bookItemID +
                               Path.GetExtension(file.File.FileName);

            var filePath = Path.Combine(directoryPath, fileName);

            Attachment at = new Attachment()
            {
                FileType = type,
                FileName = fileName,
                BookItemID = bookItemID,
                Source = filePath,
                Descryption = file.Descryption
            };

            file.File.SaveAs(filePath);
            db.Attachments.Add(at);
            db.SaveChanges();

        }

        public BookEditViewModel GetBookEditViewModel(BookItem book, ApplicationDbContext db)
        {

            BookEditViewModel bookView = new BookEditViewModel();
            var item = GetBookViewModel(book, db);
            bookView.BookItemViewModel = item;

            foreach (var file in book.Attachments)
            {
                switch (file.FileType)
                {
                    case FileType.Cover:
                        bookView.Cover = file;
                        break;
                    case FileType.TableOfContents:
                        bookView.Table = file;
                        break;
                    case FileType.Attachment:
                        bookView.OldFiles.Add(file);
                        break;
                }
            }
            return bookView;
        }

        private void DeleteCover_Table(ApplicationDbContext db, int id, FileType type)
        {
            if (db.Attachments.Any(x => x.BookItemID == id && x.FileType == type))
            {
                var item = db.Attachments.Single(x => x.BookItemID == id && x.FileType == type);
                db.Attachments.Remove(item);

                string directoryPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload"), id.ToString(), "Cover");
                var filePath = Path.Combine(directoryPath, item.FileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

            }
        }

        private void DeleteOldFile(ApplicationDbContext db, int id, List<Attachment> old)
        {
            string directoryPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload"), id.ToString());
            var files = Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly);

            var oldFiles = old.Select(x => x.Source);

            var delete = files.Except(oldFiles);

            foreach (var arg in delete)
            {
                if (File.Exists(arg))
                {
                    File.Delete(arg);
                }
                if (db.Attachments.Any(x => x.BookItemID == id && x.Source == arg))
                {
                    var item = db.Attachments.Single(x => x.BookItemID == id && x.Source == arg);
                    db.Attachments.Remove(item);
                }
            }

        }
    }
}