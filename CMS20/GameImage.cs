//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS20
{
    using System;
    using System.Collections.Generic;
    
    public partial class GameImage
    {
        public int GameImageID { get; set; }
        public Nullable<int> GameID { get; set; }
        public string DescriptImage { get; set; }
        public string SmallImagePath { get; set; }
        public string FulllImagePath { get; set; }
    
        public virtual Game Game { get; set; }
    }
}
