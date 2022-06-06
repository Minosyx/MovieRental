using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entities;
using Entities.Models;
using DataAccessLayer.Repositories.Abstract;

namespace WypozyczalnieFilmowMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDirectorRepository _directorRepository;

        public MoviesController(IMovieRepository movieRepository, ICategoryRepository categoryRepository, IDirectorRepository directorRepository)
        {
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
            _directorRepository = directorRepository;
        }

        // GET: Movies
        public async Task<ActionResult> Index()
        {
            var movies = await _movieRepository.GetMoviesAsync();
            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = await _movieRepository.GetMovieAsync(id.Value);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Create()
        {
            ViewBag.Categories = await _categoryRepository.GetCategoriesAsync();
            ViewBag.Directors = await _directorRepository.GetDirectorsAsync();
            return View();
        }

        // POST: Movies/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,DirectorId,CategoryIds,Year,Price,Image")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _movieRepository.SaveMovieAsync(movie);
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Categories = await _categoryRepository.GetCategoriesAsync();
            ViewBag.Directors = await _directorRepository.GetDirectorsAsync();
            Movie movie = await _movieRepository.GetMovieAsync(id.Value);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,DirectorId,CategoryIds,Year,Price,Image")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _movieRepository.SaveMovieAsync(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = await _movieRepository.GetMovieAsync(id.Value);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Movie movie = await _movieRepository.GetMovieAsync(id);
            await _movieRepository.DeleteMovieAsync(movie);
            return RedirectToAction("Index");
        }
    }
}
