
namespace ChaosCore.ModelBase
{
    public enum DeveloperCertification
    {
        NotCertified,
        Pending,
        Certified,
        CertificationIsDenied
    }
    public class Developer : BaseGuidEntity, IBaseGuidEntity
    {
        public virtual User User { get; set; }

        public DeveloperCertification Certification { get; set; } = DeveloperCertification.NotCertified;

        public string RealName { get; set; }

        public string CompanyName { get; set; }


    }
}
