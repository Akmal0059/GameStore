using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.WebUI.Models
{
    public class CategoryInfo
    {
        public Category Category { get; set; }
        public bool IsSelected { get; set; }
    }
}