using System;

namespace PPM.Entities
{
    public class UserToken : MyEntity
    {
        public virtual User User { get; set; }
        public virtual string AuthToken { get; set; }
        public virtual DateTime IssuedOn { get; set; }
        public virtual DateTime ExpiresOn { get; set; }
    }
}