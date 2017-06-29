
namespace Portal.Services
{
    using Portal.Entity;
    using System;
    using Phyah.EntityFramework.Services;
    using Microsoft.EntityFrameworkCore;
    using Portal.IServices;

    public class CardService : Service<Card>,ICardService
    {
        public CardService(DbContext Context) : base(Context)
        {
        }
        public CardService( ) : base(new DataContext())
        {
        }
    }
}
