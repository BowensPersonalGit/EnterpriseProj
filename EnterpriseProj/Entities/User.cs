namespace EnterpriseProj.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        //One-to-many relationship with the Job
        public int? JobId { get; set; } = null;
		public Job? Job { get; set; }  

		//Navigation Property for Many-to-Many with the Clients
		//Lets initialize this to an empty list if there is nothing in the DB
		//Appointments where this user is the practitioner
		public ICollection<Appointment>? PractitionerAppointments { get; set; } = new List<Appointment>();

		//Appointments where this user is the client
		public ICollection<Appointment>? ClientAppointments { get; set; } = new List<Appointment>();
	}
}