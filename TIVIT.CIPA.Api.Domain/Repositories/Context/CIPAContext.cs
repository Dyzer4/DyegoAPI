using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Repositories.Config;
using TIVIT.CIPA.Api.Domain.Settings;
using Action = TIVIT.CIPA.Api.Domain.Model.Action;

namespace TIVIT.CIPA.Api.Domain.Repositories.Context
{
    public class CIPAContext : DbContext
    {
        //Security
        public DbSet<User> Users { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileAction> ProfileActions { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<UserAuth> UsersAuth { get; set; }

        //Generic Log
        public DbSet<OperationLog> OperationLog { get; set; }

        //eleição
        public DbSet<Election> Elections { get; set; }

        public DbSet<ElectionSite> ElectionSites { get; set; }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Voter> Voters { get; set; }

        //cadastros, dominios e configurações
        public DbSet<Site> Sites { get; set; }

        private readonly DatabaseSettings _databaseSettings;
        private readonly IUserInfo _userInfo;

        public CIPAContext(IOptions<DatabaseSettings> databaseSettings, IUserInfo userInfo)
        {
            _databaseSettings = databaseSettings.Value;
            _userInfo = userInfo;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql(
                    "Server=estacionamento-estacionamento.b.aivencloud.com;Port=25438;Database=Tivit_db;User=avnadmin;Password=AVNS_Odl4L5CUTwVZCt6SY-3;SslMode=Required;",
                    new MySqlServerVersion(new Version(8, 0, 36))
                )
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserPermissionConfig());
            modelBuilder.ApplyConfiguration(new ProfileConfig());
            modelBuilder.ApplyConfiguration(new ProfileActionConfig());
            modelBuilder.ApplyConfiguration(new ActionConfig());
            modelBuilder.ApplyConfiguration(new UserAuthConfig());

            modelBuilder.ApplyConfiguration(new ElectionConfig());
            modelBuilder.ApplyConfiguration(new ElectionSiteConfig());
            modelBuilder.ApplyConfiguration(new CandidateConfig());
            modelBuilder.ApplyConfiguration(new VoterConfig());
            modelBuilder.ApplyConfiguration(new VoteConfig());
            modelBuilder.ApplyConfiguration(new VoterActionConfig());

            modelBuilder.ApplyConfiguration(new SiteConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var excludedProperties = new HashSet<string> { "Id", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy" };
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .ToList();

            foreach (var entry in entries)
            {
                var recordId = 0;
                var entityChanges = new List<EntityChange>();

                foreach (var property in entry.Properties)
                {
                    var oldValue = property.OriginalValue;
                    var newValue = property.CurrentValue;

                    if (property.Metadata.Name == "Id" && entry.State == EntityState.Modified)
                    {
                        recordId = Int32.Parse(oldValue?.ToString());
                    }

                    if ((property.IsModified || entry.State == EntityState.Added) &&
                        !excludedProperties.Contains(property.Metadata.Name))
                    {
                        if (entry.State == EntityState.Added || !Equals(oldValue, newValue))
                        {
                            entityChanges.Add(new EntityChange
                            {
                                Field = property.Metadata.Name,
                                Old = entry.State == EntityState.Added ? null : oldValue?.ToString(),
                                New = newValue?.ToString()
                            });
                        }
                    }
                }

                if (entityChanges.Any())
                {
                    var operationLog = new OperationLog
                    {
                        Operation = entry.State.ToString(),
                        RecordId = recordId,
                        EntityName = entry.Entity.GetType().Name,
                        UserName = _userInfo.Upn,
                        When = DateTime.UtcNow,
                        ChangesJson = JsonConvert.SerializeObject(entityChanges)
                    };

                    await OperationLog.AddAsync(operationLog, cancellationToken);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}