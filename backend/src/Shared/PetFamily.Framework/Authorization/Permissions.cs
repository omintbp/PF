namespace PetFamily.Framework.Authorization;

public static class Permissions
{
    public static class Species
    {
        public const string CreateSpecies = "species.create";
        public const string ReadSpecies = "species.read";
        public const string UpdateSpecies = "species.update";
        public const string DeleteSpecies = "species.delete";
    }

    public static class Volunteers
    {
        public const string CreateVolunteer = "volunteer.create";
        public const string ReadVolunteer = "volunteer.read";
        public const string UpdateVolunteer = "volunteer.update";
        public const string DeleteVolunteer = "volunteer.delete";
    }

    public static class Pets
    {
        public const string CreatePet = "pet.create";
        public const string ReadPet = "pet.read";
        public const string UpdatePet = "pet.update";
        public const string DeletePet = "pet.delete";
    }

    public static class VolunteerRequests
    {
        public const string CreateVolunteerRequest = "volunteer.request.create";
        public const string TakeVolunteerRequestToReview = "volunteer.request.take.to.review";
        public const string SendVolunteerRequestToRevision = "volunteer.request.send.revision";
        public const string RejectVolunteerRequest = "volunteer.request.reject";
        public const string UpdateVolunteerRequest = "volunteer.request.update";
        public const string ApproveVolunteerRequest = "volunteer.request.approve";
        
        public const string GetUnclaimedVolunteerRequests = "volunteer.request.get.unclaimed.requests";
        public const string GetVolunteerRequestsByAdminId = "volunteer.request.get.by.admin.id";
        public const string GetVolunteerRequestsByUserId = "volunteer.request.get.by.user.id";
    }
}