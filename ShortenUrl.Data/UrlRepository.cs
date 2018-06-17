using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenUrl.Data
{
    public class UrlRepository
    {
        private string _connectionString;

        public UrlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUrl(URL url)
        {
            using (var context = new UserUrlDataContext(_connectionString))
            {
                context.URLs.InsertOnSubmit(url);
                context.SubmitChanges();
            }
        }
       public URL GetUrl(string email, string url)
        {
            using (var context = new UserUrlDataContext(_connectionString))
            {
                return context.URLs.FirstOrDefault(u => u.User.Email==email && u.RealURL == url);
            }
        }
        public IEnumerable<URL> GetUrlByEmail(string email)
        {
            using (var context = new UserUrlDataContext(_connectionString))
            {
                return context.URLs.Where(u => u.User.Email == email).ToList();
            }
        }
        public URL Get(string shortenedurl)
        {
            using (var context = new UserUrlDataContext(_connectionString))
            {
                return context.URLs.FirstOrDefault(u => u.ShortenedURL == shortenedurl);
            }
        }
        public void IncrementViews(int urlId)
        {
            using (var context = new UserUrlDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Urls SET Views = Views + 1 WHERE Id = {0}", urlId);
            }
        }
    }
}
