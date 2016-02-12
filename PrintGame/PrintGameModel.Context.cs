﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrintGame
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PrintGameDataEntities : DbContext
    {
        public PrintGameDataEntities()
            : base("name=PrintGameDataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<FileShare> FileShare { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GameImage> GameImage { get; set; }
        public virtual DbSet<GameTag> GameTag { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
    
        public virtual ObjectResult<GetGameBoxImage_Result> GetGameBoxImage(Nullable<int> gameID)
        {
            var gameIDParameter = gameID.HasValue ?
                new ObjectParameter("GameID", gameID) :
                new ObjectParameter("GameID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetGameBoxImage_Result>("GetGameBoxImage", gameIDParameter);
        }
    
        public virtual ObjectResult<GetGamePage_Result> GetGamePage(Nullable<int> pageNum)
        {
            var pageNumParameter = pageNum.HasValue ?
                new ObjectParameter("PageNum", pageNum) :
                new ObjectParameter("PageNum", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetGamePage_Result>("GetGamePage", pageNumParameter);
        }
    }
}
