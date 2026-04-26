using Microsoft.EntityFrameworkCore;


namespace RM.Models.Extensions
{
    public static class EntitiesToViewExtensions
    {
        public static void ConfigureEntitiesToView(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchEngine>().ToView(null);

            modelBuilder.Entity<DynamicFormAdvanceSearch>().ToView(null);

            modelBuilder.Entity<SurveyResult>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });
            modelBuilder.Entity<StatisticsResult>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });

            modelBuilder.Entity<GeneralNumbersResult>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });

            modelBuilder.Entity<TotalVisitedByEntityResult>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });
        }
    }
}
