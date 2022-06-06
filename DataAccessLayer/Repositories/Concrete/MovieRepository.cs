using DataAccessLayer.Repositories.Abstract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Concrete
{
    public class MovieRepository : BaseRepository, IMovieRepository
    {
        public async Task<Movie> GetMovieAsync(int id)
        {
            return await context.Movies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await context.Movies.ToListAsync();
        }

        public async Task<bool> SaveMovieAsync(Movie movie)
        {
            if (movie == null)
                return false;

            try
            {
                var state = context.Entry(movie).State = movie.Id == default(int) ? EntityState.Added : EntityState.Modified;
                List<Category> categories = context.Categories.Where(c => movie.CategoryIds.Contains(c.Id)).ToList();
                if (state == EntityState.Modified)
                {
                    var mov = context.Movies.Include(c => c.Categories).FirstOrDefault(m => m.Id == movie.Id);
                    if (mov != null)
                        mov.Categories.Clear();
                }
                movie.Categories = categories;
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> DeleteMovieAsync(Movie movie)
        {
            if (movie == null)
                return false;

            context.Movies.Remove(movie);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
