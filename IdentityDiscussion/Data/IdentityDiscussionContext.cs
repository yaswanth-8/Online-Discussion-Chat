using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IdentityDiscussion.Models;

namespace IdentityDiscussion.Data
{
    public class IdentityDiscussionContext : DbContext
    {
        public IdentityDiscussionContext (DbContextOptions<IdentityDiscussionContext> options)
            : base(options)
        {
        }

        public DbSet<IdentityDiscussion.Models.Topic> Topic { get; set; } = default!;
    }
}
