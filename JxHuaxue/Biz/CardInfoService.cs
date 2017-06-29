using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phyah.Huaxue.Models;

namespace Phyah.Huaxue.Biz
{
    public class CardInfoService : ServiceBase<CardInfoes>
    {
        public CardInfoService() : base(new DataContext())
        {
        }
    }
}
