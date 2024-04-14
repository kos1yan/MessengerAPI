using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryChatExtensions
    {
        public static IEnumerable<Chat> Search(this List<Chat> chats, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return chats;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return chats.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Chat> Search(this IQueryable<Chat> chats, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return chats;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return chats.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
    }
}
