﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Library.Entities;
using Library.Model;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
	[Route("api/authors/{authorId}/books")]
	public class BooksController : Controller
	{
		private readonly ILibraryRepository _libraryRepository;

		public BooksController(ILibraryRepository libraryRepository)
		{
			_libraryRepository = libraryRepository;
		}

		[HttpGet]
		public IActionResult GetBooksForAuthor(Guid authorId)
		{
			if (!_libraryRepository.AuthorExists(authorId)) return NotFound();
			var booksForAuthorFromRepo = _libraryRepository.GetBooksForAuthor(authorId);
			var booksForAuthor = Mapper.Map<IEnumerable<BookDto>>(booksForAuthorFromRepo);
			return Ok(booksForAuthor);
		}

		[HttpGet("{id}", Name = "GetBookForAuthor")]
		public IActionResult GetBookForAuthor(Guid authorId, Guid id)
		{
			if (!_libraryRepository.AuthorExists(authorId)) return NotFound();
			var bookForAuthorFromRepo = _libraryRepository.GetBookForAuthor(authorId, id);
			if (bookForAuthorFromRepo == null) return NotFound();
			var bookForAuthor = Mapper.Map<BookDto>(bookForAuthorFromRepo);
			return Ok(bookForAuthor);
		}

		[HttpPost]
		public IActionResult CreateBookForAuthor(Guid authorId, [FromBody] BookForCreationDto book)
		{
			if (book == null) return BadRequest();
			if (!_libraryRepository.AuthorExists(authorId)) return NotFound();
			var bookEntity = Mapper.Map<Book>(book);
			_libraryRepository.AddBookForAuthor(authorId, bookEntity);
			if (!_libraryRepository.Save()) throw new Exception("Um problema aconteceu na gravação dos dados");
			var bookToReturn = Mapper.Map<BookDto>(bookEntity);
			return CreatedAtRoute("GetBookForAuthor", new {authorId, id = bookToReturn.Id}, bookToReturn);
		}
	}
}