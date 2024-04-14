using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryChannelExtensions
    {
        public static IEnumerable<Channel> Search(this List<Channel> channels, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return channels;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return channels.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Channel> Search(this IQueryable<Channel> channels, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return channels;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return channels.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
    }
}
