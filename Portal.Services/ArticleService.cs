
namespace Portal.Services
{
    using Phyah.EntityFramework.Services;
    using Portal.Entity;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;

    public class ArticleService : Service<Article>, IServices.IArticleService
    {
        public ArticleService(DbContext Context) : base(Context)
        {
        }
        public ArticleService() : base(new DataContext())
        {
        }
    }
}
