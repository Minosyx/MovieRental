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
    public class OrderRepository: BaseRepository, IOrderRepository
    {
        public async Task<Order> GetOrderAsync(int id)
        {
            return await context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<bool> SaveOrderAsync(Order order)
        {
            if (order == null)
                return false;

            try
            {
                var state = context.Entry(order).State = order.Id == default(int) ? EntityState.Added : EntityState.Modified;
                List<Movie> movies = context.Movies.Where(m => order.MovieIds.Contains(m.Id)).ToList();
                if (state == EntityState.Modified)
                {
                    var ord = context.Orders.Include(m => m.Movies).FirstOrDefault(o => o.Id == order.Id);
                    if (ord != null)
                        ord.Movies.Clear();
                }
                order.Movies = movies;
                decimal sum = 0;
                foreach (Movie movie in order.Movies)
                {
                    sum += movie.Price;
                }
                order.TotalPrice = sum;
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> DeleteOrderAsync(Order order)
        {
            if (order == null)
                return false;

            context.Orders.Remove(order);

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
