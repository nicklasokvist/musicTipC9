using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicTip171210.Models
{
    /// <summary>
    /// View model for page, containg IEnumerable selections for ListBoxes. /Nicklas 171211
    /// </summary>
    public class MusicTipViewModel
    {
        public IEnumerable<SelectListItem> Artists { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; }

        public IEnumerable<SelectListItem> Tracks { get; set; }

        public IEnumerable<SelectListItem> SelectionSeeds { get; set; }
        public IEnumerable<SelectListItem> Recommendations { get; set; }
    }
}
