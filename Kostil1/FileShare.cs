//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kostil1
{
    using System;
    using System.Collections.Generic;
    
    public partial class FileShare
    {
        public int FileShareID { get; set; }
        public int GameID { get; set; }
        public string FileShareName { get; set; }
        public string FileShareUrl1 { get; set; }
        public string FileShareUrl2 { get; set; }
        public string FileShareUrl3 { get; set; }
        public string FileShareUrl4 { get; set; }
        public string FileShareDesc { get; set; }
        public Nullable<long> FileShareSize { get; set; }
        public Nullable<System.DateTime> FileShareExpire1 { get; set; }
        public Nullable<System.DateTime> FileShareExpire2 { get; set; }
        public Nullable<System.DateTime> FileShareExpire3 { get; set; }
        public Nullable<System.DateTime> FileShareExpire4 { get; set; }
    }
}
