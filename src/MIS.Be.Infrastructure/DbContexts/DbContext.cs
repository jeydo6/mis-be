using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Infrastructure.DataContexts;

internal sealed class DbContext : DataConnection
{
	private static readonly MappingSchema _mappingSchema = CreateMappingSchema();

	public DbContext(string connectionString) : base(CreateDataOptions(connectionString))
	{
	}

	public ITable<Dispanserization> Dispanserizations => this.GetTable<Dispanserization>();
	public ITable<Employee> Employees => this.GetTable<Employee>();
	public ITable<Patient> Patients => this.GetTable<Patient>();
	public ITable<Research> Researches => this.GetTable<Research>();
	public ITable<Resource> Resources => this.GetTable<Resource>();
	public ITable<Room> Rooms => this.GetTable<Room>();
	public ITable<Specialty> Specialties => this.GetTable<Specialty>();
	public ITable<TimeItem> TimeItems => this.GetTable<TimeItem>();
	public ITable<VisitItem> VisitItems => this.GetTable<VisitItem>();

	private static MappingSchema CreateMappingSchema()
	{
		var mappingSchema = new MappingSchema();
		var builder = new FluentMappingBuilder(mappingSchema);

		builder
			.Entity<Dispanserization>()
			.HasTableName(nameof(Dispanserization))
			.HasIdentity(e => e.Id);

		builder
			.Entity<Employee>()
			.HasTableName(nameof(Employee))
			.HasIdentity(e => e.Id);

		builder
			.Entity<Patient>()
			.HasTableName(nameof(Patient))
			.HasIdentity(e => e.Id);

		builder
			.Entity<Research>()
			.HasTableName(nameof(Research))
			.HasIdentity(e => e.Id);

		builder
			.Entity<Resource>()
			.HasTableName(nameof(Resource))
			.HasIdentity(e => e.Id);

		builder
			.Entity<Room>()
			.HasTableName(nameof(Room))
			.HasIdentity(e => e.Id);

		builder
			.Entity<Specialty>()
			.HasTableName(nameof(Specialty))
			.HasIdentity(e => e.Id);

		builder
			.Entity<TimeItem>()
			.HasTableName(nameof(TimeItem))
			.HasIdentity(e => e.Id);

		builder
			.Entity<VisitItem>()
			.HasTableName(nameof(VisitItem))
			.HasIdentity(e => e.Id);

		builder.Build();

		return mappingSchema;
	}

	private static DataOptions CreateDataOptions(string connectionString)
		=> new DataOptions()
            .UsePostgreSQL(connectionString)
			.UseMappingSchema(_mappingSchema);
}
