using System;
using System.Collections.Generic;
using Library.Entities;
using Library.Model;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
	[Route("api/authorcollections")]
	public class AuthorCollectionsController : Controller
    {
	    private readonly ILibraryRepository _libraryRepository;

	    public AuthorCollectionsController(ILibraryRepository libraryRepository)
	    {
		    _libraryRepository = libraryRepository;
	    }

	    [HttpPost]
	    public IActionResult CreateAuthorCollection([FromBody] IEnumerable<AuthorForCreationDto> authorCollection)
	    {
		    if (authorCollection == null) return BadRequest();

		    var authorEntities = AutoMapper.Mapper.Map<IEnumerable<Author>>(authorCollection);

		    foreach (var author in authorEntities)
		    {
			    _libraryRepository.AddAuthor(author);
		    }

			if(!_libraryRepository.Save()) throw new Exception("Create an author collection failed on save.");

		    return Ok();
	    }

		[HttpGet("({ids})")]
		public IActionResult GetAuthorCollection(IEnumerable<Guid> ids)
		{
			
		}
		 
    }
}
