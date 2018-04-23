using System;
using System.Collections.Generic;
using AutoMapper;
using Library.Entities;
using Library.Model;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
	[Route("api/authors")]
	public class AuthorsController : Controller
	{
		private readonly ILibraryRepository _libraryRepository;

		public AuthorsController(ILibraryRepository libraryRepository)
		{
			_libraryRepository = libraryRepository;
		}

		[HttpGet]
		public IActionResult GetAuthors()
		{
			var authorsFromRepo = _libraryRepository.GetAuthors();
			var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
			return Ok(authors);
		}

		[HttpGet("{id}", Name = "GetAuthor")]
		public IActionResult GetAuthor(Guid id)
		{
			var authorsFromRepo = _libraryRepository.GetAuthor(id);
			if (authorsFromRepo == null) return NotFound();
			var author = Mapper.Map<AuthorDto>(authorsFromRepo);
			return Ok(author);
		}

		[HttpPost]
		public IActionResult CreateAuthor([FromBody] AuthorForCreationDto author)
		{
			if (author == null) return BadRequest();
			var authotEntity = Mapper.Map<Author>(author);
			_libraryRepository.AddAuthor(authotEntity);
			if (!_libraryRepository.Save()) throw new Exception("Um problema acont4eceu na gravação dos dados");
			var authorToReturn = Mapper.Map<AuthorDto>(authotEntity);
			return CreatedAtRoute("GetAuthor", new {id = authorToReturn.Id}, authorToReturn);
		}
	}
}