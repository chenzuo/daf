using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Generators;
using DAF.Social;
using DAF.Social.Models;

namespace DAF.Social.LocalProvider
{
    public class RepoSocialProvider : ISocialProvider
    {
        private ITransactionManager trans;
        private IIdGenerator generator;
        private IRepository<Org> repoOrg;
        private IRepository<Person> repoPerson;
        private IRepository<School> repoSchool;
        private IRepository<Contact> repoContact;
        private IRepository<PersonCertificate> repoPersonCert;
        private IRepository<PersonContact> repoPersonContact;
        private IRepository<PersonLink> repoPersonLink;
        private IRepository<StudyResume> repoStudyResume;
        private IRepository<WorkResume> repoWorkResume;
        
        public RepoSocialProvider(ITransactionManager trans, IIdGenerator generator,
            IRepository<Org> repoOrg, IRepository<Person> repoPerson, IRepository<School> repoSchool,
            IRepository<Contact> repoContact, IRepository<PersonCertificate> repoPersonCert, IRepository<PersonContact> repoPersonContact,
            IRepository<PersonLink> repoPersonLink, IRepository<StudyResume> repoStudyResume, IRepository<WorkResume> repoWorkResume)
        {
            this.trans = trans;
            this.generator = generator;
            this.repoOrg = repoOrg;
            this.repoPerson = repoPerson;
            this.repoSchool = repoSchool;
            this.repoContact = repoContact;
            this.repoPersonCert = repoPersonCert;
            this.repoPersonContact = repoPersonContact;
            this.repoPersonLink = repoPersonLink;
            this.repoStudyResume = repoStudyResume;
            this.repoWorkResume = repoWorkResume;
        }

        public Person GetPerson(string personId, bool loadAllInfo = false)
        {
            throw new NotImplementedException();
        }

        public bool SavePerson(Person person)
        {
            throw new NotImplementedException();
        }

        public bool RemovePerson(string personId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersonLink> GetPersonLinks(string personId, PersonLinkType[] linkTypes = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetPersonLinkIds(string personId, PersonLinkType[] linkTypes = null)
        {
            throw new NotImplementedException();
        }

        public bool SavePersonLink(PersonLink personLink)
        {
            throw new NotImplementedException();
        }

        public bool RemovePersonLink(string personId, string linkPersonId)
        {
            throw new NotImplementedException();
        }
    }
}
